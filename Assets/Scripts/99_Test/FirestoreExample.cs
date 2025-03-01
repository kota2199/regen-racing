using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreExample : MonoBehaviour
{
    FirebaseFirestore db;

    private bool isConnected;

    void Start()
    {
        isConnected = false;

        // Firebaseの初期化
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Firestore is ready!");
                isConnected = true;
                // データ登録の例
                AddFutureChoice("eco");
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    private void Update()
    {
        if (isConnected && Input.GetKey(KeyCode.Space))
        {
            float charge = Random.Range(0, 101);
            float speed = Random.Range(0, 201);
            float rate = Random.Range(0, 101);
            AddValue(charge, speed, rate);
        }
    }

    void AddValue(float charge, float speed, float regenRate)
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

    void AddFutureChoice(string choice)
    {
        // 登録するデータを定義
        Dictionary<string, object> test = new Dictionary<string, object>
        {
            { "future", choice },
        };

        // Firestoreにデータを追加
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
}
