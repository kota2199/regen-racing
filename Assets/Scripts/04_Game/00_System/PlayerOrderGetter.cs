using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOrderGetter : MonoBehaviour
{
    [SerializeField]
    private RaceData raceData;

    [SerializeField]
    private int myPos;

    [SerializeField]
    private Text posText, ordinalNumText;

    private string ordinalNum;

    void Start()
    {
        raceData.GetCarInfo("Player").Position = 4;
    }

    // Update is called once per frame
    void Update()
    {
        myPos = raceData.GetCarInfo("Player").Position;
        switch (myPos)
        {
            case 1:
                ordinalNum = "st";
                break;
            case 2:
                ordinalNum = "nd";
                break;
            case 3:
                ordinalNum = "rd";
                break;
            default:
                ordinalNum = "th";
                break;

        }
        posText.text = myPos.ToString();
        ordinalNumText.text = ordinalNum;
    }
}
