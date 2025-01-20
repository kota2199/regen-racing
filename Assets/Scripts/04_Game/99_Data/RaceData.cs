using UnityEngine;
using System.Collections.Generic;

// Car情報を保持するクラス
[System.Serializable]
public class CarInfo
{
    public string CarName;
/*  public int Lap;
    public int PassedPoint;*/
    public int Position;
    public float time;
    public int colorIndex;
    public int choiceIndex;
    public bool isFinished;
    public float ecoPoint;
    public float totalTime;
}

// ScriptableObjectを作成するクラス
[CreateAssetMenu(fileName = "RaceData", menuName = "ScriptableObjects/RaceData", order = 1)]
public class RaceData : ScriptableObject
{
    public List<CarInfo> cars;

    public int playerColorIndex, playerChoiceIndex;

    public bool isPlay = false;

    public void Initialize()
    {
        cars.Clear();
        GameObject[] carArray = GameObject.FindGameObjectsWithTag("Car");
        foreach(GameObject car in carArray)
        {
            CarInfo carInfo = new CarInfo();
            carInfo.CarName = car.name;
            cars.Add(carInfo);
        }
    }
    public CarInfo GetCarInfo(string carName)
    {
        return cars.Find(car => car.CarName == carName);
    }

    /*public int GetCarLap(string carName)
    {
        CarInfo car = GetCarInfo(carName);
        return car.Lap;
    }

    public int GetCarPassedPointNumber(string carName)
    {
        CarInfo car = GetCarInfo(carName);
        return car.PassedPoint;
    }

    public void UpdateCarInfo(string carName, int lap, int passedPoint, float time)
    {
        CarInfo car = GetCarInfo(carName);
        if (car != null)
        {
            car.Lap = lap;
            car.PassedPoint = passedPoint;
            car.time = time;
        }
    }
*/
    public void UpdateCarPositionsByTime()
    {
        // CarInfoリストをtimeの小さい順にソート
        List<CarInfo> sortedCars = new List<CarInfo>(cars);
        sortedCars.Sort((carA, carB) => carA.Position.CompareTo(carB.Position));
        sortedCars.Sort((carA, carB) => carB.isFinished.CompareTo(carA.isFinished));

        // ソートされたリストの順番に基づいてPositionを設定
        for (int i = 0; i < sortedCars.Count; i++)
        {
            sortedCars[i].Position = i + 1; // Positionは1から始まる
        }

        // ソートされたリストを元のcarsリストに反映
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i] = sortedCars[i];
        }
    }

    public void UpdateCarColorInfo(string name, int colorIndex)
    {
        CarInfo car = GetCarInfo(name);
        if (car != null)
        {
            car.colorIndex = colorIndex;
        }
    }

    public void UpdateCarChoiceNumber(string name, int index)
    {
        CarInfo car = GetCarInfo(name);
        if(car != null)
        {
            car.choiceIndex = index;
        }
    }

    public void UpdateFinishTime(string carName, float time, int position)
    {
        CarInfo car = GetCarInfo(carName);
        if(car != null)
        {
            car.Position = position;
            car.time = time;
            car.isFinished = true;
        }
    }

    public void UpdateFinishStatus(string name, float totalTime)
    {
        CarInfo car = GetCarInfo(name);
        if (car != null)
        {
            car.isFinished = true;
            car.totalTime = totalTime;
            switch (car.choiceIndex)
            {
                case 0:
                    car.ecoPoint = -10.00f;
                    car.totalTime = car.time + car.ecoPoint;
                    break;
                case 1:
                    car.ecoPoint = 0.00f;
                    car.totalTime = car.time + car.ecoPoint;
                    break;
                case 2:
                    car.ecoPoint = 10.00f;
                    car.totalTime = car.time + car.ecoPoint;
                    break;
                default:
                    Debug.LogError("Out of Index");
                    break;
            }

            //UpdateCarPositionsByTime();
        }
    }
}
