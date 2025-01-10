using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    [SerializeField]
    private TimeGapManager gapManager;

    public int checkPointNumber;

    // Start is called before the first frame update
    void Start()
    {
        gapManager = GameObject.FindObjectOfType<TimeGapManager>();
    }

    public void CarPassed(int currentLap, GameObject car)
    {
        gapManager.PassPoint(car, checkPointNumber, this.gameObject.tag, currentLap);
    }
}
