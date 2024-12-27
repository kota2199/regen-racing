using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseUpdater : MonoBehaviour
{
    FirebaseFirestore db;

    private bool isConnected;

    [SerializeField]
    private RaceData raceData;

    [SerializeField]
    private SpeedCheck speedCheck;

    [SerializeField]
    private BatterySystem batterySystem;

    [SerializeField]
    private CountDown countDown;

    [SerializeField]
    private float updateInterval = 1;
    private float passedTime = 0;

    void Start()
    {
        isConnected = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Firestore is ready!");
                isConnected = true;
                AddFutureChoice(raceData.playerChoiceIndex.ToString());
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    void Update()
    {
        if (isConnected && countDown.isPlay)
        {
            passedTime += Time.deltaTime;
            if(updateInterval < passedTime)
            {
                UpdateData();
                passedTime = 0;
            }
        }
    }

    private void UpdateData()
    {
        float charge = batterySystem.remainBattery;
        float speed = speedCheck.speed;
        float rate = batterySystem.chargeRate;
        AddValue(charge, speed, rate);
    }

    public void AddFutureChoice(string choice)
    {
        Dictionary<string, object> test = new Dictionary<string, object>
        {
            { "future", choice },
        };

        db.Collection("test").Document("test").SetAsync(test).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data successfully added to Firestore!");
            }
            else
            {
                Debug.LogError($"Failed to add data: {task.Exception}");
            }
        });
    }

    public void AddValue(float charge, float speed, float regenRate)
    {
        Dictionary<string, object> chargeUpdates = new Dictionary<string, object>
        {
            { "charge", FieldValue.ArrayUnion(charge)},
            { "speed", FieldValue.ArrayUnion(speed)},
            { "rate", FieldValue.ArrayUnion(regenRate)}
        };

        db.Collection("test").Document("test").UpdateAsync(chargeUpdates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Succes updating");
            }
            else
            {
                Debug.LogError($"Failed to add data: {task.Exception}");
            }
        });
    }
}
