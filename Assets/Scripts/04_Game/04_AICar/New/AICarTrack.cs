﻿using System.Collections;
using UnityEngine;

public class AICarTrack : MonoBehaviour
{
    //This script will move the AI waypoint tracker when the AI car triggers a point. The AI car will always follow the tracker
    //and the tracker will take the position of the next point that is placed on the racetrack
    private GameObject Point;
    [SerializeField]
    private int CurrentPoint;
    [SerializeField]private GameObject aiCar;

    private Vector3 carPos, pos;

    [SerializeField]
    private float approachDistance;


    private void Start()
    {
        CurrentPoint = 1;//at the start of the race the AI car will drive to the first AI waypoint object
        Point = GameObject.Find("Point" + CurrentPoint);    //the script will search the next point automatically
        this.transform.position = Point.transform.position; //then, it will get the position of the point and move the tracker to there 
    }

    private void Update()
    {
        carPos = aiCar.transform.position;
        pos = transform.position;

        float carPosX = carPos.x;
        float thisPosX = pos.x;
        float carPosZ = carPos.z;
        float thisPosZ = pos.z;

        if (Mathf.Abs(carPosX - thisPosX) < approachDistance && Mathf.Abs(carPosZ - thisPosZ) < approachDistance)
        {
            SwitchToNextTarget();
        }
    }

    private void  SwitchToNextTarget()
    {
        Point = GameObject.Find("Point" + CurrentPoint);    //the script will search the next point automatically
        this.transform.position = Point.transform.position; //then, it will get the position of the point and move the tracker to there 
        CurrentPoint += 1;//the AI point that the AI car will follow now will be the next one
        if (GameObject.Find("Point" + CurrentPoint) == null)//if the next point doesn't exists, that means that the last point was passed
        {
            Debug.Log("Target Point is Null");
            CurrentPoint = 1;//so it will return to the first one
        }
        //now that we completed the next AI point assignment:
    }

    public void ReverseTarget()
    {
        CurrentPoint -= 1;//the AI point that the AI car will follow now will be the next one
        Point = GameObject.Find("Point" + CurrentPoint);    //the script will search the next point automatically
        this.transform.position = Point.transform.position; //then, it will get the position of the point and move the tracker to there
        CurrentPoint += 1;//the AI point that the AI car will follow now will be the next one
        if (GameObject.Find("Point" + CurrentPoint) == null)//if the next point doesn't exists, that means that the last point was passed
        {
            Debug.Log("Target Point is Null");
            CurrentPoint = 1;//so it will return to the first one
        }
    }
}