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

    [SerializeField] float[] DriveWheels = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] float[] SteerWheels = new float[] { 0f, 0f, 1.0f, 1.0f };
    [SerializeField] float SpeedHandleLimit = 1.0f;

    private float restrictor;

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
    }

    // Update is called once per frame
    void Update()
    {
        ControllInput();

        CarControll();

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
                    brakeAmount = 0;
                }
                if(Input.GetAxis(brakeButton) > 0.1f)
                {
                    acceleAmount = 0;
                    brakeAmount = Input.GetAxis(brakeButton);
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

        if (InputVector.y > 0)
        {
            //batterySystem.remainBattery -= 1.0f * restrictor * Time.deltaTime;
            batterySystem.UseBattery(0.3f * restrictor * Time.deltaTime);
            regenerativeBrakeAmount = 0;
        }
        if (InputVector.y <= 0)
        {
            if (speedCheck.speed >= 10)
            {
                //batterySystem.remainBattery += 2f * restrictor * Time.deltaTime;
                batterySystem.ChargeBattery(2f * restrictor * Time.deltaTime);
                regenerativeBrakeAmount = 150;
            }
            else
            {
                regenerativeBrakeAmount = 0;
            }
        }
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