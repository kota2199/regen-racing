using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseUpdater : MonoBehaviour
{
    FirebaseFirestore db;

    private bool isConnected;

    void Start()
    {
        isConnected = false;

        // Firebase�̏�����
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Firestore is ready!");
                isConnected = true;
                // �f�[�^�o�^�̗�
                AddFutureChoice("eco");
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    void Update()
    {
        if (isConnected && Input.GetKey(KeyCode.Space))
        {
            float charge = Random.Range(0, 101);
            float speed = Random.Range(0, 201);
            float rate = Random.Range(0, 101);
            AddValue(charge, speed, rate);
        }
    }

    public void AddFutureChoice(string choice)
    {
        // �o�^����f�[�^���`
        Dictionary<string, object> test = new Dictionary<string, object>
        {
            { "future", choice },
        };

        // Firestore�Ƀf�[�^��ǉ�
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
