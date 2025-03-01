using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject manager;

    private CountDown countDown;

    private BatterySystem batterySystem;

    private SpeedCheck speedCheck;

    private BoostModeManager boostManager;


    public Rigidbody RB { get; private set; }
    [SerializeField] Transform CenterOfMass;
    [SerializeField] WheelCollider[] Wheel;
    [SerializeField] Transform[] Obj;

    [SerializeField] string XAxisName = "Horizontal";
    [SerializeField] string YAxisName = "Vertical";
    [SerializeField] KeyCode BrakeKey = KeyCode.Space;

    [SerializeField] Vector2 InputVector;
    [SerializeField] float Brake = 0;

    public float AccelPower = 1000f;
    [SerializeField] float HandleAngle = 45f;
    [SerializeField] float BrakePower = 1000f;
    [SerializeField] float regenerativeBrakeAmount = 0.0f;
    [SerializeField] float DownForce;

    [SerializeField] float[] DriveWheels = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[] SteerWheels = new float[] { 0f, 0f, 1.0f, 1.0f };
    [SerializeField] float SpeedHandleLimit = 1.0f;

    private float restrictor;

    //calculate deceleration
    private Vector3 previousVelocity;
    private float previousTime;
    private float decelerationMagnitude;

    //Manage Battery
    [SerializeField]
    private float batteryUseAmount, batteryChargeAmount;

    //Reverase
    [SerializeField, Range(-1, 1)]
    private int reverse = 1;

    //pad
    public bool padControll = true;

    [SerializeField]
    private string acceleButton, brakeButton, steerButton, reverseButton;

    private float acceleAmount, brakeAmount;

    void Awake()
    {
        countDown = manager.GetComponent<CountDown>();

        batterySystem = GetComponent<BatterySystem>();

        speedCheck = GetComponent<SpeedCheck>();

        boostManager = GetComponent<BoostModeManager>();

        Wheel = GetComponentsInChildren<WheelCollider>();
        RB = GetComponent<Rigidbody>();
        RB.centerOfMass = CenterOfMass.localPosition;

        Obj = new Transform[Wheel.Length];
        for (int i = 0; i < Wheel.Length; i++)
        {
            Obj[i] = Wheel[i].transform.GetChild(0);
        }
    }

    void Start()
    {
        padControll = IsGamepadConnected();

        previousVelocity = RB.velocity;
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        ControllInput();

        CarControll();

        CalcDownForce();

        //ForDebug
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (padControll)
            {
                padControll = false;
                Debug.Log("Controll : KEYBOARD (ReturnKey for Pad)");
            }
            else
            {
                padControll = true;
                Debug.Log("Controll : PAD (ReturnKey for Keyboard)");
            }
        }
    }

    private void FixedUpdate()
    {
        float currentTime = Time.time;
        Vector3 currentVelocity = RB.velocity;

        // 経過時間を計算
        float deltaTime = currentTime - previousTime;
        if (deltaTime > 0)
        {
            // 減速率（加速度）を計算
            Vector3 acceleration = (currentVelocity - previousVelocity) / deltaTime;

            // 減速率の大きさ（スカラー値）を計算
            decelerationMagnitude = -Vector3.Dot(acceleration, previousVelocity.normalized);

            // 表示（減速のみを表示）
            if (decelerationMagnitude > 0)
            {
                //Debug.Log("減速率: " + decelerationMagnitude + " m/s²");
            }
        }

        // 現在の値を次のフレーム用に更新
        previousVelocity = currentVelocity;
        previousTime = currentTime;
    }

    private void ControllInput()
    {
        if (countDown.isPlay)
        {
            //PadControll
            if (padControll)
            {
                if (Input.GetAxis(acceleButton) > 0.1f)
                {
                    acceleAmount = Input.GetAxis(acceleButton);
                }
                else
                {
                    acceleAmount = 0f;
                }

                if (Input.GetAxis(acceleButton) < -0.1f)
                {
                    brakeAmount = -Input.GetAxis(acceleButton);
                }
                else
                {
                    brakeAmount = 0f;
                }

                InputVector = new Vector2(Input.GetAxis(steerButton), acceleAmount * reverse);
                Brake = brakeAmount * BrakePower;

                if (Input.GetButtonDown(reverseButton) && reverse == 1)
                {
                    reverse = -1;
                }
                else if (Input.GetButtonDown(reverseButton) && reverse == -1)
                {
                    reverse = 1;
                }
            }
            //KeyboardControll
            else
            {
                InputVector = new Vector2(Input.GetAxis(XAxisName), Input.GetAxis(YAxisName));
                Brake = Input.GetKey(BrakeKey) ? BrakePower : 0f;
            }
        }
    }

    private void CarControll()
    {
        float _sh = RB.velocity.magnitude * SpeedHandleLimit;
        restrictor = batterySystem.restrictor;

        for (int i = 0; i < Wheel.Length; i++)
        {
            if (batterySystem.remainBattery <= 0)
            {
                Wheel[i].motorTorque = 0;
            }
            else
            {
                Wheel[i].motorTorque = InputVector.y * DriveWheels[i] * AccelPower * boostManager.addBoostPower * (restrictor / 4) - regenerativeBrakeAmount;
            }

            Wheel[i].steerAngle = InputVector.x * SteerWheels[i] * HandleAngle;
            Wheel[i].brakeTorque = Brake;

            Vector3 _pos;
            Quaternion _dir;
            Wheel[i].GetWorldPose(out _pos, out _dir);
            Obj[i].position = _pos;
            Obj[i].rotation = _dir;
        }

        //Use and Charge Battery
        if (Mathf.Abs(InputVector.y) > 0)
        {
            batterySystem.UseBattery(Mathf.Abs(InputVector.y) * batteryUseAmount * restrictor * Time.deltaTime);
            regenerativeBrakeAmount = 0;
        }
        if (speedCheck.speed >= 10)
        {
            batterySystem.ChargeBattery(decelerationMagnitude * batteryChargeAmount * restrictor * Time.deltaTime);
            regenerativeBrakeAmount = 150;
        }
        else
        {
            regenerativeBrakeAmount = 0;
        }
    }

    private void CalcDownForce()
    {
        float dowForcePower = DownForce * RB.velocity.magnitude;
        RB.AddRelativeForce(new Vector3(0, -dowForcePower, 0));
    }

    public static bool IsGamepadConnected()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string name in joystickNames)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return true;
            }
        }
        return false;
    }
}