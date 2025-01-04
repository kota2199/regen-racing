using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatterySystem : MonoBehaviour
{
    private CarSystem carSystem;

    [SerializeField]
    private Text batteryText, perText;

    [SerializeField]
    private Color lowBatteryColor, highBatteryColor;

    [SerializeField]
    private Image[] batteryIndicators;

    [SerializeField]
    private Sprite fullImage, lowImage, emptyImage;

    [SerializeField]
    private Image chargeStatusImage;

    [SerializeField]
    private Sprite[] chargeStatusSprites;

    public float remainBattery, restrictor;

    const float minRestrictor = 1;
    const float maxRestrictor = 4;

    public float chargeAmount, totalAmount, chargeRate;

    [SerializeField]
    private Image[] manageIndicators;

    [SerializeField]
    private Sprite[] availableImage, unavailableImage;


    //needle
    [SerializeField]
    private Image needle;

    [SerializeField]
    private Vector2 needleRotation;

    [SerializeField]
    private float maxSpeedOfMeter;

    [SerializeField]
    private float maxNeedleAngle;

    private float needleAngle;

    private RectTransform needleRect;

    [SerializeField]
    private string restrictorUp, restrictorDown;

    //Audio
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip upSound, downSound;

    //Human or AI
    private bool humanCar = false;

    void Start()
    {
        carSystem = GetComponent<CarSystem>();

        remainBattery = 100;
        restrictor = 4;

        if(GetComponent<GameModeManager>().carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
            HumanInitialize();
        }
        else
        {
            humanCar = false;
        }
    }

    private void HumanInitialize()
    {
        needleRect = needle.gameObject.GetComponent<RectTransform>();

        audioSource = GetComponent<AudioSource>();

        UpdateManageIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        BaterryLimit();

        if (humanCar)
        {
            if (Input.GetButtonDown(restrictorDown) || Input.GetKeyDown(KeyCode.Q))
            {
                ChangeRestrictor(-1);
            }
            if (Input.GetButtonDown(restrictorUp) || Input.GetKeyDown(KeyCode.E))
            {
                ChangeRestrictor(1);
            }

            UpdateUI();
            CalcChargeRate();
        }
    }

    private void FixedUpdate()
    {
        if (humanCar)
        {
            UpdateBatteryIndicator();
        }
    }

    public void ChargeBattery(float amount)
    {
        remainBattery += amount;
        chargeAmount += amount;
        //For calc chargeRate;
    }

    public void UseBattery(float amount)
    {
        remainBattery -= amount;
        totalAmount -= amount;
    }

    private void ChangeRestrictor(int amount)
    {
        restrictor += amount;
        RestrictorLimit();
        UpdateManageIndicator();

        audioSource.Stop();

        if(amount > 0)
        {
            audioSource.PlayOneShot(upSound);
        }
        else
        {
            audioSource.PlayOneShot(downSound);
        }
    }

    private void BaterryLimit()
    {
        if (remainBattery <= 0)
        {
            remainBattery = 0;
        }
        if (remainBattery > 100)
        {
            remainBattery = 100;
        }
    }

    private void RestrictorLimit()
    {
        // ?????l???????????????l???n??
        restrictor = System.Math.Min(restrictor, maxRestrictor);
        // ?????l?????????????????l???n??
        restrictor = System.Math.Max(restrictor, minRestrictor);
    }

    private void UpdateUI()
    {
        //text
        batteryText.text = remainBattery.ToString("f0");

        //needle's angle
        needleAngle = -1 * remainBattery * 1.8f + maxNeedleAngle;
        needleRect.rotation = Quaternion.Euler(0.0f, 0.0f, needleAngle);

        //textcolor
        if (remainBattery > 30)
        {
            batteryText.color = highBatteryColor;
            perText.color = highBatteryColor;
        }
        else
        {
            batteryText.color = lowBatteryColor;
            perText.color = lowBatteryColor;
        }

    }

    private void UpdateBatteryIndicator()
    {
        for (int i = 0; i < 10; i++)
        {
            if (remainBattery > i * 10)
            {
                if(remainBattery > 30)
                {
                    batteryIndicators[i].sprite = fullImage;
                }
                else
                {
                    batteryIndicators[i].sprite = lowImage;
                }
            }
            else
            {
                batteryIndicators[i].sprite = emptyImage;
            }
        }
    }

    private void UpdateManageIndicator()
    {
        for (int i = 0; i < manageIndicators.Length; i++)
        {
            if(i != restrictor - 1)
            {
                manageIndicators[i].sprite = unavailableImage[i];
            }
            else
            {
                manageIndicators[i].sprite = availableImage[i];
            }
        }
    }

    private void CalcChargeRate()
    {
        chargeRate = (chargeAmount / Mathf.Abs(totalAmount)) * 100;
    }
}
