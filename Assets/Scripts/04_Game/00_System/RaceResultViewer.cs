using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RaceResultViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUi,finishUi;
        
    [SerializeField]
    private RectTransform finishPanel;

    [SerializeField]
    private Text t_Position, t_OrdinalNum, t_Time, t_WaitTime;

    private RacePositionManager racePositionManager;

    [SerializeField]
    private Vector3 defaultPos, targetPos;

    [SerializeField]
    private float animTime;

    [SerializeField]
    private float waitTime;

    private float waitCount;

    [SerializeField]
    private FadeInOut fadeInOut;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip goalSound;

    private bool finished = false;

    private void Awake()
    {
        racePositionManager = GetComponent<RacePositionManager>();
        finished = false;
    }

    private void Start()
    {
        finishPanel.anchoredPosition = defaultPos;
        waitCount = waitTime;
        finishUi.SetActive(false);
    }

    private void Update()
    {
        if(finished && waitCount >= 0)
        {
            waitCount -= Time.deltaTime;
        }
        t_WaitTime.text = "ランキング表示まであと" + waitCount.ToString("f0") + "秒";
    }
    public void DisplayResult(float totalTime)
    {
        finished = true;
        audioSource.PlayOneShot(goalSound);
        int pos = racePositionManager.GetPosition("Player");
        gameUi.SetActive(false);
        finishUi.SetActive(true);

        t_Position.text = pos.ToString();
        t_OrdinalNum.text = MakeOrdinalNumber(pos.ToString());
        t_Time.text = FormatToMMSSSSS(totalTime);

        finishPanel.DOAnchorPos(targetPos, animTime).SetEase(Ease.OutExpo);

        StartCoroutine(WaitResult());
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
    private string FormatToMMSSSSS(float totalSeconds)
    {
        // ?????v?Z
        int minutes = (int)(totalSeconds / 60);
        // ?b???v?Z
        float seconds = totalSeconds % 60;

        // MM:SS.SSS?`????????????????
        return $"{minutes}:{seconds:00.000}";
    }

    private IEnumerator WaitResult()
    {
        yield return new WaitForSeconds(waitTime);
        fadeInOut.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("06_Result");
    }
}
