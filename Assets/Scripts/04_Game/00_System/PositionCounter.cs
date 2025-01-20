using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionCounter : MonoBehaviour
{
    public int currentCheckpoint = 0; // 現在のチェックポイント
    public float distanceToNextCheckpoint = 0f; // 次のチェックポイントまでの距離

    [SerializeField]
    private GameObject checkPointParent;
    
    [SerializeField]
    private Transform[] checkpoints; // チェックポイントのリスト

    private GameModeManager gameModeManager;

    [SerializeField]
    private Text positionText, ordinalNumText;

    [SerializeField]
    private RacePositionManager posManager;

    private int currentLap = 0; // 現在の周回数
    private float progress = 0f; // コース内の進捗

    [SerializeField]
    private RaceData raceData;

    private void Awake()
    {
        gameModeManager = GetComponent<GameModeManager>();
    }

    private void Start()
    {
        checkpoints = new Transform[checkPointParent.transform.childCount];
        int count = 0; // 番目を表示するためのもの
        foreach (Transform point in checkPointParent.transform)
        {
            checkpoints[count] = point; // 順番に子オブジェクトを取得
            count++;
        }
    }

    private void Update()
    {
        if (raceData.isPlay)
        {
            // 現在位置から次のチェックポイントまでの距離を計算
            if (currentCheckpoint < checkpoints.Length - 1)
            {
                distanceToNextCheckpoint = Vector3.Distance(transform.position, checkpoints[currentCheckpoint + 1].position);
            }

            if(gameModeManager.carOwner == GameModeManager.CarOwner.Human)
            {
                UpdateUI();
            }
        }
    }

    public void UpdateCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex == currentCheckpoint + 1)
        {
            currentCheckpoint = checkpointIndex;
        }
    }

    public float GetProgress()
    {
        // 総進捗を計算（例: 現在のチェックポイント + 次のチェックポイントまでの進行割合）
        if(currentCheckpoint < checkpoints.Length - 1)
        {
            float currentProgress = currentCheckpoint + (1 - (distanceToNextCheckpoint / Vector3.Distance(checkpoints[currentCheckpoint].position, checkpoints[currentCheckpoint + 1].position)));
            return currentProgress;
        }
        else
        {
            float currentProgress = currentCheckpoint + (1 - (distanceToNextCheckpoint / Vector3.Distance(checkpoints[currentCheckpoint].position, checkpoints[0].position)));
            return currentProgress;
        }
    }

    // CarProgressスクリプトにワールド座標での比較を追加
    public float GetProgressWithPosition(Vector3 forwardDirection)
    {
        float progress = GetProgress();
        float positionInDirection = Vector3.Dot(transform.position, forwardDirection);
        return progress + positionInDirection * 0.001f; // 微調整用の係数を追加
    }

    private void UpdateUI()
    {
        int myPos = posManager.GetPosition(gameObject.name);
        positionText.text = myPos.ToString();
        ordinalNumText.text = MakeOrdinalNumber(myPos.ToString());
    }

    private string MakeOrdinalNumber(string num)
    {
        switch (num)
        {
            case "1":
                return "st";

            case "2":
                return "nd";

            case "3":
                return "rd";

            default:
                return "th";
        }
    }

    public void SetProgress(float newProgress)
    {
        if (newProgress < progress) // 進捗がリセットされたら周回+1
        {
            currentLap++;
        }
        progress = newProgress;
    }

    public int GetLapCount()
    {
        return currentLap;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ControlLine")
        {
            currentLap++;
            currentCheckpoint = 0;
        }
    }
}
