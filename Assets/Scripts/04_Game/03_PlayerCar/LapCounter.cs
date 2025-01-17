using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    [SerializeField]
    private RaceData raceData;

    private GameModeManager gameModeManager;

    [SerializeField]
    private int defaultGrid;

    //Lap
    [SerializeField]
    private GameObject checkPointsParent;

    [SerializeField]
    private Transform[] checkPoints;

    [SerializeField]
    private int maxLap;

    public int lapCount;

    private bool lapZero = true;

    [SerializeField]
    private Text lapText;

    //Time
    public bool isCount;

    public float timer, totalTime;

    [SerializeField]
    private Text t_timer;

    //Finished
    [SerializeField]
    private GameObject finishedPanel;

    public bool isFinished;

    //ForReplacePlayer
    private ReplaceController playerReplacer;

    //ForReplaceAI
    private AIAutoReverse aiReplacer;

    //Human or AI
    private bool humanCar = false;

    private bool firstPassedCheckPoint = false;

    [SerializeField]
    private PopUpManager popUpManager;

    [SerializeField]
    private RaceResultViewer raceResultViewer;

    private bool finalLapPopped = false;

    // Start is called before the first frame update

    private void Awake()
    {
        gameModeManager = GetComponent<GameModeManager>();

        if (gameModeManager.carOwner == GameModeManager.CarOwner.Human)
        {
            humanCar = true;
        }
        else
        {
            humanCar = false;
        }

        if (humanCar)
        {
            playerReplacer = GetComponent<ReplaceController>();
        }
        else
        {
            if (GetComponent<AIAutoReverse>())
            {
                aiReplacer = GetComponent<AIAutoReverse>();
            }
        }

        checkPoints = new Transform[checkPointsParent.transform.childCount];

        for (var i = 0; i < checkPoints.Length; ++i)
        {
            checkPoints[i] = checkPointsParent.transform.GetChild(i);
        }

        lapCount = 1;

        isCount = false;
        timer = 0.0f;
        totalTime = 0.0f;

        isFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter();

        if (!firstPassedCheckPoint)
        {
            //SetFirstGrid();
        }

        if(humanCar)
        {
            UpdateUI();
        }
    }

    private void SetFirstGrid()
    {
        string carName = this.gameObject.name;
        raceData.GetCarInfo(carName).Position = defaultGrid;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CheckPoint")
        {
            firstPassedCheckPoint = true;

            //SetReplacePoint
            if (humanCar)
            {
                playerReplacer.SetReplacePoint(other.gameObject.transform);
            }
            else
            {
                aiReplacer.SetReplacePoint(other.gameObject.transform);
            }
        }

        if(other.gameObject.tag == "ControlLine")
        {
            if (lapZero)
            {
                lapZero = false;
            }
            else
            {
                //laped
                timer = 0.0f;
                if (lapCount >= maxLap)
                {
                    Finished();
                }
                else
                {
                    lapCount++;
                }

                if (humanCar && lapCount == maxLap && !finalLapPopped)
                {
                    popUpManager.PopUp(3);
                    finalLapPopped = true;
                }
            }
            

            //other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);
        }
    }

    private void TimeCounter()
    {
        isCount = CountDown.instance.isPlay;
        if (isCount)
        {
            timer += Time.deltaTime;
            totalTime += Time.deltaTime;
        }
    }

    private void UpdateUI()
    {
        lapText.text = lapCount.ToString() + "/" + maxLap.ToString();

        t_timer.text = FormatToMMSSSSS(timer);

        finishedPanel.SetActive(isFinished);
    }

    public static string FormatToMMSSSSS(float totalSeconds)
    {
        // 分を計算
        int minutes = (int)(totalSeconds / 60);
        // 秒を計算
        float seconds = totalSeconds % 60;

        // MM:SS.SSS形式の文字列を返す
        return $"{minutes}:{seconds:00.000}";
    }

    private void Finished()
    {
        isFinished = true;
        raceData.UpdateFinishStatus(this.gameObject.name, totalTime);

        if (humanCar)
        {
            this.GetComponent<CarSystem>().enabled = false;
            this.GetComponent<AICarController>().enabled = true;
            raceResultViewer.DisplayResult(totalTime);
        }
    }
}
