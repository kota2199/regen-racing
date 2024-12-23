using System.Collections.Generic;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class FirestoreExample : MonoBehaviour
{
    FirebaseFirestore db;

    void Start()
    {
        // Firebaseの初期化
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Firestore is ready!");

                // データ登録の例
                AddData();
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    void AddData()
    {
        // 登録するデータを定義
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "name", "NIMINIMINIMI" },
            { "age", 22 },
            { "email", "nimi@example.com" }
        };

        // Firestoreにデータを追加
        db.Collection("users").Document("user1").SetAsync(user).ContinueWithOnMainThread(task =>
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
