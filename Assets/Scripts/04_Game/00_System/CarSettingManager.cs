using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CarSettingManager : MonoBehaviour
    {
    [SerializeField]
    private GameObject playerCar;

    [SerializeField]
    private GameObject[] aICars;

    [SerializeField]
    private float choice1MaxTorque, choice1CarWeight;
    [SerializeField]
    private float choice2MaxTorque, choice2CarWeight;
    [SerializeField]
    private float choice3MaxTorque, choice3CarWeight;

    [SerializeField]
    private RaceData raceData;

    private void Awake()
    {
        raceData.Initialize();
    }
    // Start is called before the first frame update6 
    void Start()
    {
        for (int i = 0; i < raceData.cars.Count; i++)
        {
            if (raceData.cars[i].CarName == "Player")
            {
                    raceData.cars[i].choiceIndex = raceData.playerChoiceIndex;
            }
        }
        RandomSetAICar();
        SetPlayerCar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RandomSetAICar()
    {
        for (int i = 0; i < aICars.Length; i++)
        {
            int carMode = Random.Range(0, 3);
                /*switch (carMode)
                {
                    case 0:
                        aICars[i].GetComponent<AICarController>().AccelPower = choice1MaxTorque;
                        aICars[i].GetComponent<AICarControll>().m_FullTorqueOverAllWheels = choice1MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice1CarWeight;
                        break;

                    case 1:
                        aICars[i].GetComponent<AICarController>().AccelPower = choice2MaxTorque;
                        aICars[i].GetComponent<AICarControll>().m_FullTorqueOverAllWheels = choice2MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice2CarWeight;
                        break;

                    case 2:
                        aICars[i].GetComponent<AICarController>().AccelPower = choice3MaxTorque;
                        aICars[i].GetComponent<AICarControll>().m_FullTorqueOverAllWheels = choice3MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice3CarWeight;
                        break;
                }*/
                switch (carMode)
                {
                    case 0:
                        aICars[i].GetComponent<AutoDriveController>().maxMotorTorque = choice1MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice1CarWeight;
                        break;

                    case 1:
                        aICars[i].GetComponent<AutoDriveController>().maxMotorTorque = choice2MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice2CarWeight;
                        break;

                    case 2:
                        aICars[i].GetComponent<AutoDriveController>().maxMotorTorque = choice3MaxTorque;
                        aICars[i].GetComponent<Rigidbody>().mass = choice3CarWeight;
                        break;
                }

                raceData.UpdateCarChoiceNumber(aICars[i].name, carMode);
        }
    }

    private void SetPlayerCar()
    {
        int playerMode = raceData.playerChoiceIndex;
        switch (playerMode)
        {
            case 0:
                playerCar.GetComponent<CarSystem>().AccelPower = choice1MaxTorque;
                playerCar.GetComponent<Rigidbody>().mass = choice1CarWeight;
                break;

            case 1:
                playerCar.GetComponent<CarSystem>().AccelPower = choice2MaxTorque;
                playerCar.GetComponent<Rigidbody>().mass = choice2CarWeight;
                break;

            case 2:
                playerCar.GetComponent<CarSystem>().AccelPower = choice3MaxTorque;
                playerCar.GetComponent<Rigidbody>().mass = choice3CarWeight;
                break;
        }
    }
}

}
    