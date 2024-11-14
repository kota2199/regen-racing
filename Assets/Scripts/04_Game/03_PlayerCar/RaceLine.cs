using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Rigidbody playerCar;
    public Transform checkpointsParent; // チェックポイントの親オブジェクト
    public Color lowSpeedColor = Color.green;
    public Color mediumSpeedColor = Color.yellow;
    public Color highSpeedColor = Color.red;

    private Transform[] checkpoints; // コースのチェックポイント
    private float[] recommendedSpeeds; // 各セグメントの推奨速度
    private List<Vector3> smoothPoints; // 補間後の滑らかなポイント
    private float currentRecommendedSpeed = 0f;

    void Start()
    {
        // 子オブジェクトからチェックポイントを自動取得
        if (checkpointsParent != null)
        {
            int count = checkpointsParent.childCount;
            checkpoints = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                checkpoints[i] = checkpointsParent.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("Checkpoints parent is not assigned.");
            return;
        }

        // 推奨速度配列の初期化
        recommendedSpeeds = new float[checkpoints.Length];
        for (int i = 0; i < checkpoints.Length; i++)
        {
            recommendedSpeeds[i] = checkpoints[i].GetComponent<SpeedCheckpoint>().recommendedSpeed;
        }

        // 補間された滑らかなラインの生成
        smoothPoints = GenerateSmoothPoints();
        lineRenderer.positionCount = smoothPoints.Count;
        lineRenderer.SetPositions(smoothPoints.ToArray());
    }

    void Update()
    {
        UpdateRecommendedSpeed();
        UpdateLineColorBasedOnSpeed();
    }

    List<Vector3> GenerateSmoothPoints()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < checkpoints.Length; i++)
        {
            // 各チェックポイント間をスムーズに繋ぐ
            Vector3 p0 = checkpoints[(i - 1 + checkpoints.Length) % checkpoints.Length].position;
            Vector3 p1 = checkpoints[i].position;
            Vector3 p2 = checkpoints[(i + 1) % checkpoints.Length].position;
            Vector3 p3 = checkpoints[(i + 2) % checkpoints.Length].position;

            // Catmull-Rom スプラインによる補間
            for (float t = 0; t < 1; t += 0.05f) // 0.05f は補間ステップ（調整可能）
            {
                Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                points.Add(point);
            }
        }

        return points;
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Catmull-Rom スプラインの計算
        float t2 = t * t;
        float t3 = t2 * t;

        float a0 = -0.5f * t3 + t2 - 0.5f * t;
        float a1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
        float a2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float a3 = 0.5f * t3 - 0.5f * t2;

        return a0 * p0 + a1 * p1 + a2 * p2 + a3 * p3;
    }

    void UpdateRecommendedSpeed()
    {
        float minDistance = float.MaxValue;
        Transform nearestCheckpoint = null;

        // プレイヤーに最も近いチェックポイントを探す
        foreach (var checkpoint in checkpoints)
        {
            float distance = Vector3.Distance(playerCar.transform.position, checkpoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCheckpoint = checkpoint;
            }
        }

        if (nearestCheckpoint != null)
        {
            // チェックポイントの推奨速度を取得
            currentRecommendedSpeed = nearestCheckpoint.GetComponent<SpeedCheckpoint>().recommendedSpeed;
        }
    }

    void UpdateLineColorBasedOnSpeed()
    {
        float playerSpeed = playerCar.velocity.magnitude * 3.6f;

        // グラデーションの初期化
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        // 速度に応じてラインの色を決定
        Color startColor = lowSpeedColor;
        Color endColor;

        if (playerSpeed <= currentRecommendedSpeed * 0.8f)
        {
            endColor = lowSpeedColor;
        }
        else if (playerSpeed <= currentRecommendedSpeed)
        {
            endColor = mediumSpeedColor;
        }
        else
        {
            endColor = highSpeedColor;
        }

        // グラデーションの開始と終了位置の色設定
        colorKeys[0].color = startColor;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = endColor;
        colorKeys[1].time = 1.0f;
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        // グラデーションを設定
        gradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient;
    }
}
