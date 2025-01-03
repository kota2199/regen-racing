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

    public int maxCheckPoint, checkedNum;

    [SerializeField]
    private int maxLap;

    public int lapCount;

    [SerializeField]
    private Text lapText;

    //Time
    public bool isCount;

    private float timer, selfBestTime, totalTime;

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
    private ResultManager resultManager;

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

        maxCheckPoint = checkPoints.Length;
        checkedNum = 0;

        lapCount = 1;

        isCount = false;
        timer = 0.0f;
        selfBestTime = 0.0f;

        isFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter();

        if (!firstPassedCheckPoint)
        {
            SetFirstGrid();
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

            if (other.gameObject.name == checkPoints[checkedNum].name)
            {
                checkedNum++;
                other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);

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
        }

        if(other.gameObject.tag == "ControlLine" && maxCheckPoint <= checkedNum)
        {
            //laped
            FastestCheck(timer, lapCount);
            timer = 0.0f;

            if(lapCount >= maxLap)
            {
                Finished();
            }
            else
            {
                lapCount++;
            }

            other.gameObject.GetComponent<PositionChecker>().CarPassed(lapCount, this.gameObject);
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

    private void FastestCheck(float lapTime, int lap)
    {
        if(lap == 1 || selfBestTime > lapTime)
        {
            selfBestTime = lapTime;
        }
    }

    private void UpdateUI()
    {
        lapText.text = lapCount.ToString() + "/" + maxLap.ToString();

        finishedPanel.SetActive(isFinished);
    }

    private void Finished()
    {
        isFinished = true;
        raceData.UpdateFinishStatus(this.gameObject.name);
        resultManager.CalcResult(this.gameObject.name);

        if (humanCar)
        {
            this.GetComponent<CarSystem>().enabled = false;
            this.GetComponent<AICarController>().enabled = true;
        }
    }
}
