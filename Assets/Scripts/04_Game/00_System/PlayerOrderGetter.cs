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
    private Text posText;

    private string myPosText;

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
                myPosText = "1st";
                break;
            case 2:
                myPosText = "2nd";
                break;
            case 3:
                myPosText = "3rd";
                break;
            default:
                myPosText = myPos.ToString() + "th";
                break;

        }
        posText.text = myPosText;
    }
}
