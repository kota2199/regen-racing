using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private RaceData raceDate;

    [SerializeField]
    private List<GameObject> Uis;

    [SerializeField]
    private string[] futureOptions;

    List<CarInfo> carList;

    private int finishedCarCount;

    private void Start()
    {
        carList = raceDate.cars;
        finishedCarCount = 0;

        for(int i = 0; i < raceDate.cars.Count; i++)
        {
            Uis[i].transform.Find("t_Mode").GetComponent<Text>().text = futureOptions[raceDate.cars[i].choiceIndex];
            if(raceDate.cars[i].CarName == "Player")
            {
                Uis[i].transform.Find("im_You").gameObject.SetActive(true);
            }
        }
    }

    public void CalcResult(string goalCarName)
    {
        //車がコントロールラインを通過したら実行
        foreach(CarInfo car in carList)
        {
            if(car.CarName == goalCarName)
            {
                Uis[finishedCarCount].transform.Find("t_Pos").GetComponent<Text>().text = car.Position.ToString();
                Uis[finishedCarCount].transform.Find("t_Time").GetComponent<Text>().text = car.time.ToString();
                Uis[finishedCarCount].transform.Find("t_Eco").GetComponent<Text>().text = car.ecoPoint.ToString();
                Uis[finishedCarCount].transform.Find("t_Total").GetComponent<Text>().text = car.totalTime.ToString();
                finishedCarCount++;
            }
        }
    }
}
