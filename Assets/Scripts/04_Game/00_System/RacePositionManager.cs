using System.Collections.Generic;
using UnityEngine;

public class RacePositionManager : MonoBehaviour
{
    [SerializeField] private List<PositionCounter> cars;
    [SerializeField] private RaceData raceData;
    private Dictionary<string, int> positionCache = new Dictionary<string, int>();

    private void Start()
    {
        if (cars == null || cars.Count == 0)
        {
            cars = new List<PositionCounter>(FindObjectsOfType<PositionCounter>());
        }

        InvokeRepeating(nameof(UpdateRacePositions), 0f, 1f); // 1秒に1回順位更新
    }

    private void UpdateRacePositions()
    {
        if (raceData.isPlay)
        {
            cars.RemoveAll(car => car == null);

            // ✅ 値をキャッシュ化
            Dictionary<PositionCounter, (int lap, float progress)> progressCache = new();
            foreach (var car in cars)
            {
                if (car != null)
                {
                    progressCache[car] = (car.GetLapCount(), car.GetProgress());
                }
            }

            // ✅ nullチェック & ソート
            cars.Sort((car1, car2) =>
            {
                if (car1 == null) return 1;
                if (car2 == null) return -1;

                var (lap1, progress1) = progressCache[car1];
                var (lap2, progress2) = progressCache[car2];

                if (lap1 != lap2)
                    return lap2.CompareTo(lap1);

                return progress2.CompareTo(progress1);
            });

            // ✅ 順位キャッシュ更新
            positionCache.Clear();
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i] != null)
                {
                    positionCache[cars[i].gameObject.name] = i + 1;
                }
            }
        }
    }

    public int GetPosition(string carName)
    {
        return positionCache.TryGetValue(carName, out int pos) ? pos : -1;
    }
}
