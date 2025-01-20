using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Vehicles.Car
{
    //[RequireComponent(typeof (CarController))]
    public class CarAIControl : MonoBehaviour
    {
        public enum BrakeCondition
        {
            NeverBrake,                 // 車は常に全開で加速し続ける
            TargetDirectionDifference,  // 車はターゲットの進行方向の変化に応じてブレーキをかける
                                        // ルートベースのAIに適しており、コーナーで減速するために使用される
            TargetDistance,             // 車はターゲットに近づくにつれてブレーキをかける（ターゲットの方向に関係なく）
                                        // 静止しているターゲットに向かい、到着した際に停止するような用途に適している
        }

        // このスクリプトは、ユーザー制御スクリプトと同じ方法で車のコントローラーに入力を提供する
        // そのため、特別な物理演算やアニメーションのトリックを使用せずに車を「運転」する

        // "wandering"（ふらつき）は、AI車の動きをより人間らしくし、ロボットのように見えないようにする
        // 走行中に速度や方向がわずかに変化するようにする

        [SerializeField] [Range(0, 1)] private float m_CautiousSpeedFactor = 0.05f;               // 最大限の注意を払う場合の最大速度の割合
        [SerializeField] [Range(0, 180)] private float m_CautiousMaxAngle = 50f;                  // コーナーの角度がこの値を超えると最大限の注意が必要とみなす
        [SerializeField] private float m_CautiousMaxDistance = 100f;                              // 距離に基づいた注意が必要になる開始距離
        [SerializeField] private float m_CautiousAngularVelocityFactor = 30f;                     // AIが自身の角速度をどの程度考慮して加速を緩めるか（スピン時の制御）
        [SerializeField] private float m_SteerSensitivity = 0.05f;                                // AIが目的の方向へ向くためのステアリング感度
        [SerializeField] private float m_AccelSensitivity = 0.04f;                                // AIが目的の速度に到達するための加速感度
        [SerializeField] private float m_BrakeSensitivity = 1f;                                   // AIが目的の速度に到達するためのブレーキ感度
        [SerializeField] private float m_LateralWanderDistance = 3f;                              // 車がターゲットに向かう際に左右にふらつく最大距離
        [SerializeField] private float m_LateralWanderSpeed = 0.1f;                               // ふらつきの速度
        [SerializeField] [Range(0, 1)] private float m_AccelWanderAmount = 0.1f;                  // 加速のふらつき度合い
        [SerializeField] private float m_AccelWanderSpeed = 0.1f;                                 // 加速のふらつきの変動速度
        [SerializeField] private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance; // AIの加減速の基準
        [SerializeField] private bool m_Driving;                                                  // AIが現在運転中か停止中か
        [SerializeField] private GameObject TargetGameObject;
        [SerializeField] private Transform m_Target;                                              // 目的地のターゲット
        [SerializeField] private bool m_StopWhenTargetReached;                                    // ターゲット到達時に停止するか
        [SerializeField] private float m_ReachTargetThreshold = 2;                                // 目的地に到達したとみなす距離のしきい値

        private float m_RandomPerlin;             // AI車がランダムにふらつくためのPerlinノイズ値
        //private CarController m_CarController;    // 操作対象のCarControllerの参照
        private AICarControll m_CarControll;
        private float m_AvoidOtherCarTime;        // 直前に衝突した車を回避するための時間
        private float m_AvoidOtherCarSlowdown;    // 衝突を回避するために減速する割合
        private float m_AvoidPathOffset;          // 進行方向に対して回避のために左右どちらにオフセットするか
        private Rigidbody m_Rigidbody;

        private void Awake()
        {
            // CarControllerの参照を取得
            //m_CarController = GetComponent<CarController>();
            m_CarControll = GetComponent<AICarControll>();

            // Perlinノイズに基づくランダム値を設定
            m_RandomPerlin = Random.value * 100;

            m_Rigidbody = GetComponent<Rigidbody>();

            m_Target = TargetGameObject.GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            if (m_Target == null || !m_Driving)
            {
                // 車を停止させるためにハンドブレーキを使用
                m_CarControll.Move(0, 0, -1f, 1f);
            }
            else
            {
                Vector3 fwd = transform.forward;
                if (m_Rigidbody.velocity.magnitude > m_CarControll.MaxSpeed * 0.1f)
                {
                    fwd = m_Rigidbody.velocity;
                }

                float desiredSpeed = m_CarControll.MaxSpeed;

                // 減速の必要性を判定
                switch (m_BrakeCondition)
                {
                    case BrakeCondition.TargetDirectionDifference:
                        {
                            // 進行方向の変化を考慮して減速
                            float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
                            float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle, Mathf.Max(spinningAngle, approachingCornerAngle));
                            desiredSpeed = Mathf.Lerp(m_CarControll.MaxSpeed, m_CarControll.MaxSpeed * m_CautiousSpeedFactor, cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.TargetDistance:
                        {
                            // ターゲットへの距離に応じて減速
                            Vector3 delta = m_Target.position - transform.position;
                            float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);
                            float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;
                            float cautiousnessRequired = Mathf.Max(Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                            desiredSpeed = Mathf.Lerp(m_CarControll.MaxSpeed, m_CarControll.MaxSpeed * m_CautiousSpeedFactor, cautiousnessRequired);
                            break;
                        }

                    case BrakeCondition.NeverBrake:
                        break;
                }

                // 衝突回避処理:
                if (Time.time < m_AvoidOtherCarTime)
                {
                    desiredSpeed *= m_AvoidOtherCarSlowdown;
                    m_Target.position += m_Target.right * m_AvoidPathOffset;
                }

                float accel = Mathf.Clamp((desiredSpeed - m_CarControll.CurrentSpeed) * (desiredSpeed < m_CarControll.CurrentSpeed ? m_BrakeSensitivity : m_AccelSensitivity), -1, 1);
                accel *= (1 - m_AccelWanderAmount) + (Mathf.PerlinNoise(Time.time * m_AccelWanderSpeed, m_RandomPerlin) * m_AccelWanderAmount);

                Vector3 localTarget = transform.InverseTransformPoint(m_Target.position);
                float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                float steer = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(m_CarControll.CurrentSpeed);

                // 車の制御を実行
                m_CarControll.Move(steer, accel, accel, 0f);
            }
        }
    }
}
