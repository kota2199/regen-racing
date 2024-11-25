using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private RaceData raceDate;

    [SerializeField]
    private Text p1Name, p1Time, p1Eco, p1Total;

    [SerializeField]
    private Text p2Name, p2Time, p2Eco, p2Total;
    
    [SerializeField]
    private Text p3Name, p3Time, p3Eco, p3Total;

    [SerializeField]
    private Text p4Name, p4Time, p4Eco, p4Total;

    public void CalcResult(string goalCarName)
    {
        //車がコントロールラインを通過したら実行
        List<CarInfo> carList = raceDate.cars;

        foreach(CarInfo car in carList)
        {
            if (car.isFinished)
            {
                switch (car.Position)
                {
                    case 1:
                        p1Name.text = car.CarName;
                        p1Time.text = car.time.ToString("f3");
                        p1Eco.text = car.ecoPoint.ToString();
                        p1Total.text = car.totalTime.ToString("f3");
                        break;

                    case 2:
                        p2Name.text = car.CarName;
                        p2Time.text = car.time.ToString("f3");
                        p2Eco.text = car.ecoPoint.ToString();
                        p2Total.text = car.totalTime.ToString("f3");
                        break;

                    case 3:
                        p3Name.text = car.CarName;
                        p3Time.text = car.time.ToString("f3");
                        p3Eco.text = car.ecoPoint.ToString();
                        p3Total.text = car.totalTime.ToString("f3");
                        break;

                    case 4:
                        p4Name.text = car.CarName;
                        p4Time.text = car.time.ToString("f3");
                        p4Eco.text = car.ecoPoint.ToString();
                        p4Total.text = car.totalTime.ToString("f3");
                        break;
                }
            }
        }
    }
}
