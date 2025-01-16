using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacePositionManager : MonoBehaviour
{
    [SerializeField]
    private List<PositionCounter> cars; // 参加車両のリスト

    private PositionCounter[] car;

    [SerializeField]
    private RaceData raceData;

    private void Start()
    {
        foreach(var car in raceData.cars)
        {
            cars.Add(GameObject.Find(car.CarName).GetComponent<PositionCounter>());
        }
    }

    private void Update()
    {
        if (raceData.isPlay)
        {
            // 順位計算
            cars.Sort((car1, car2) => car2.GetProgress().CompareTo(car1.GetProgress()));

            // 順位の表示
            for (int i = 0; i < cars.Count; i++)
            {
                Debug.Log($"順位 {i + 1}: {cars[i].name}");
            }
        }
    }

    public int GetPosition(string carName)
    {
        //int mypos = cars.IndexOf(carName);
        int mypos = 0;
        return mypos;
    }
}
