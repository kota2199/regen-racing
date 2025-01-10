using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCarController : MonoBehaviour
{
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

    //Reverase
    [SerializeField, Range(-1, 1)]
    private int reverse = 1;

    //pad
    public bool padControll = true;

    [SerializeField]
    private string acceleButton, brakeButton, steerButton, reverseButton;

    private float acceleAmount, brakeAmount;

    // Start is called before the first frame update
    void Awake()
    {
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
        padControll = true;
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
        //PadControll
        if (padControll)
        {
            if (Input.GetAxis(acceleButton) < 0.1f)
            {
                acceleAmount = -Input.GetAxis(acceleButton);
                brakeAmount = 0;
            }
            else if (Input.GetAxis(acceleButton) > -0.1f)
            {
                acceleAmount = 0;
                brakeAmount = 1;
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

    private void CarControll()
    {
        float _sh = RB.velocity.magnitude * SpeedHandleLimit;

        for (int i = 0; i < Wheel.Length; i++)
        {
            Wheel[i].motorTorque = InputVector.y * DriveWheels[i] * AccelPower;

            Wheel[i].steerAngle = InputVector.x * SteerWheels[i] * HandleAngle;
            Wheel[i].brakeTorque = Brake;

            Vector3 _pos;
            Quaternion _dir;
            Wheel[i].GetWorldPose(out _pos, out _dir);
            Obj[i].position = _pos;
            Obj[i].rotation = _dir;
        }
    }
}