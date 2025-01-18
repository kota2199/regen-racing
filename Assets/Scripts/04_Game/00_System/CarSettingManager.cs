using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
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
            switch (carMode)
            {
                case 0:
                    aICars[i].GetComponent<AICarController>().AccelPower = choice1MaxTorque;
                    aICars[i].GetComponent<Rigidbody>().mass = choice1CarWeight;
                    break;

                case 1:
                    aICars[i].GetComponent<AICarController>().AccelPower = choice2MaxTorque;
                    aICars[i].GetComponent<Rigidbody>().mass = choice2CarWeight;
                    break;

                case 2:
                    aICars[i].GetComponent<AICarController>().AccelPower = choice3MaxTorque;
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
