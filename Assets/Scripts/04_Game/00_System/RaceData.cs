using UnityEngine;
using System.Collections.Generic;

// Car情報を保持するクラス
[System.Serializable]
public class CarInfo
{
    public string CarName;
    public int Lap;
    public int Position;
    public int PassedPoint;
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

    public void ClearData()
    {
        cars.Clear();
    }

    public CarInfo GetCarInfo(string carName)
    {
        return cars.Find(car => car.CarName == carName);
    }

    public int GetCarLap(string carName)
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

    public void UpdateCarPositionsByTime()
    {
        // CarInfoリストをtimeの小さい順にソート
        List<CarInfo> sortedCars = new List<CarInfo>(cars);
        sortedCars.Sort((carA, carB) => carA.time.CompareTo(carB.time));
        sortedCars.Sort((carA, carB) => carB.PassedPoint.CompareTo(carA.PassedPoint));
        sortedCars.Sort((carA, carB) => carB.Lap.CompareTo(carA.Lap));

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
            Debug.Log("changed");
        }
    }

    public void UpdateCarChoiceNumber(string name, int index)
    {
        CarInfo car = GetCarInfo(name);
        if(car != null)
        {
            car.choiceIndex = index;
            Debug.Log("ChangeCarMode : Car" + car.CarName + " is " + index);
        }
    }

    public void UpdateFinishStatus(string name)
    {
        CarInfo car = GetCarInfo(name);
        if (car != null)
        {
            car.isFinished = true;
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

            UpdateCarPositionsByTime();
        }
    }
}
