using System.Collections;
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
        float carPosY = carPos.y;
        float thisPosY = pos.y;

        Debug.Log(aiCar.name + "'s posX gap is" + Mathf.Abs(carPosX - thisPosX).ToString());
        Debug.Log(aiCar.name + "'s posZ gap is" + Mathf.Abs(carPosY - thisPosY).ToString());

        if (Mathf.Abs(carPosX - thisPosX) < approachDistance && Mathf.Abs(carPosY - thisPosY) < approachDistance)
        {
            Debug.Log(aiCar.name + "'s target is switched.");
            StartCoroutine(SwitchToNextTarget());
        }
    }

    //IEnumerator OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject == aiCar)//If the AICar collider is detected
    //    {
    //        Point = GameObject.Find("Point" + CurrentPoint);    //the script will search the next point automatically
    //        this.transform.position = Point.transform.position; //then, it will get the position of the point and move the tracker to there 
    //        this.GetComponent<BoxCollider>().enabled = false;//the collider will disable to avoid triggering twice the AI point
    //        CurrentPoint += 1;//the AI point that the AI car will follow now will be the next one
    //        if (GameObject.Find("Point" + CurrentPoint) == null)//if the next point doesn't exists, that means that the last point was passed
    //        {
    //            CurrentPoint = 1;//so it will return to the first one
    //        }
    //        //now that we completed the next AI point assignment:
    //        yield return new WaitForSeconds(0.1f);
    //        this.GetComponent<BoxCollider>().enabled = true;//we can turn on the collider again, because it will be in the next point position already
    //    }
    //    //you can add as many points as you need from the Unity hirearchy, remember to put "Point" and the number next to it (i.e: Point13)
    //    //then, you can freely move the transform to move the point wherever you want and the tracker will move to the car, making the AI car follow it
    //}

    IEnumerator SwitchToNextTarget()
    {
        Point = GameObject.Find("Point" + CurrentPoint);    //the script will search the next point automatically
        this.transform.position = Point.transform.position; //then, it will get the position of the point and move the tracker to there 
        this.GetComponent<BoxCollider>().enabled = false;//the collider will disable to avoid triggering twice the AI point
        CurrentPoint += 1;//the AI point that the AI car will follow now will be the next one
        if (GameObject.Find("Point" + CurrentPoint) == null)//if the next point doesn't exists, that means that the last point was passed
        {
            Debug.Log("Target Point is Null");
            CurrentPoint = 1;//so it will return to the first one
        }
        //now that we completed the next AI point assignment:
        yield return null;
            //this.GetComponent<BoxCollider>().enabled = true;//we can turn on the collider again, because it will be in the next point position already
        }
}