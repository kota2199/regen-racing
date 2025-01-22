using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private RaceData raceDate;

    [SerializeField]
    private List<GameObject> Uis;

    [SerializeField]
    private RectTransform[] positions;


    [SerializeField]
    private string[] futureOptions;

    [SerializeField]
    private Color[] futureColorCode;

    [SerializeField]
    private int[] ecoPoint;

    [SerializeField]
    private Sprite[] carImage;

    [SerializeField]
    private FadeInOut fadeInOut;
    [SerializeField]
    private AudioClip time, eco ,total, afterTotal, decide;

    [SerializeField]
    private float displayWaitTime = 2f;

    [SerializeField]
    private float countDuration = 1f;

    private bool isDisplayFinished = false;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        raceDate.UpdateCarPositionsByTime();
        ResetPosition();
        StartCoroutine(DisplayResult());
    }

    private IEnumerator DisplayResult()
    {
        //Display Image & Mode
        for (int i = 0; i < raceDate.cars.Count; i++)
        {
            Text modeText = Uis[i].transform.Find("t_Mode").GetComponent<Text>();
            modeText.text = futureOptions[raceDate.cars[i].choiceIndex];
            modeText.color = futureColorCode[raceDate.cars[i].choiceIndex];

            if (raceDate.cars[i].CarName == "Player")
            {
                Uis[i].transform.Find("im_You").gameObject.SetActive(true);
            }
            else
            {
                Uis[i].transform.Find("im_You").gameObject.SetActive(false);
            }

            Uis[i].transform.Find("im_Car").GetComponent<Image>().sprite = carImage[raceDate.cars[i].colorIndex];
        }

        yield return new WaitForSeconds(displayWaitTime + 1);

        //Display Time
        for (int i = 0; i < raceDate.cars.Count; i++)
        {
            if (raceDate.cars[i].isFinished)
            {
                StartCoroutine(CountUp(Uis[i].transform.Find("t_Time").GetComponent<Text>(), 0, raceDate.cars[i].time));
                //Uis[i].transform.Find("t_Time").GetComponent<Text>().text = FormatToMMSSSSS(raceDate.cars[i].time);
            }
            else
            {
                Uis[i].transform.Find("t_Time").GetComponent<Text>().text = "DNF";
            }

            audioSource.PlayOneShot(time);
        }

        yield return new WaitForSeconds(displayWaitTime);

        //Display Eco
        for (int i = 0; i < raceDate.cars.Count; i++)
        {
            if (raceDate.cars[i].isFinished)
            {
                StartCoroutine(CountUp(Uis[i].transform.Find("t_Eco").GetComponent<Text>(), 0, ecoPoint[raceDate.cars[i].choiceIndex]));
                //Uis[i].transform.Find("t_Eco").GetComponent<Text>().text = ecoPoint[raceDate.cars[i].choiceIndex].ToString();
            }
            else
            {
                Uis[i].transform.Find("t_Eco").GetComponent<Text>().text = "DNF";
            }

            audioSource.PlayOneShot(eco);
        }
        yield return new WaitForSeconds(displayWaitTime);

        //Display Total
        for (int i = 0; i < raceDate.cars.Count; i++)
        {
            if (raceDate.cars[i].isFinished)
            {
                raceDate.cars[i].totalTime = TotalTime(raceDate.cars[i].choiceIndex, raceDate.cars[i].time);

                StartCoroutine(CountUp(Uis[i].transform.Find("t_Total").GetComponent<Text>(), 0, raceDate.cars[i].totalTime));
                //Uis[i].transform.Find("t_Total").GetComponent<Text>().text = FormatToMMSSSSS(raceDate.cars[i].totalTime);
            }
            else
            {
                Uis[i].transform.Find("t_Total").GetComponent<Text>().text = "DNF";
            }
            audioSource.PlayOneShot(total);
        }

        raceDate.UpdateCarPositionsByTotalTime();

        yield return new WaitForSeconds(1f);
        ResetPosition();
        audioSource.PlayOneShot(afterTotal);

        isDisplayFinished = true;
    }

    private string FormatToMMSSSSS(float totalSeconds)
    {
        if(totalSeconds < 0)
        {
            // 分を計算
            int minutes = -(int)(totalSeconds / 60);
            // 秒を計算
            float seconds = Mathf.Abs(totalSeconds) % 60;

            // MM:SS.SSS形式の文字列を返す
            return "-" + $"{minutes}:{seconds:00.000}";
        }
        else
        {
            // 分を計算
            int minutes = (int)(totalSeconds / 60);
            // 秒を計算
            float seconds = totalSeconds % 60;

            // MM:SS.SSS形式の文字列を返す
            return $"{minutes}:{seconds:00.000}";
        }
    }

    private float TotalTime(int index, float time)
    {
        float total;

        switch (index)
        {
            case 0:
                total = time + ecoPoint[0];
                return total;
            case 1:
                total = time + ecoPoint[1];
                return total;
            case 2:
                total = time + ecoPoint[2];
                return total;
            default:
                return 0;
        }
    }

    private void ResetPosition()
    {
        for (int i = 0; i < raceDate.cars.Count; i++)
        {
            Uis[i].GetComponent<RectTransform>().DOAnchorPos(positions[raceDate.cars[i].Position - 1].anchoredPosition, 0.5f).SetEase(Ease.InSine);
        }
    }

    private IEnumerator CountUp(Text timeText, float startNum, float targetNum)
    {
        float num = startNum;
        float elapsedTime = 0;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            num = Mathf.FloorToInt(Mathf.Lerp(0, targetNum, elapsedTime / countDuration));
            timeText.text = FormatToMMSSSSS(num);

            if (countDuration <= elapsedTime)
            {
                yield break;
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (isDisplayFinished && Input.GetButtonDown("Fire_0") || Input.GetKeyDown(KeyCode.C))
        {
            audioSource.PlayOneShot(decide);
            fadeInOut.FadeOut();
            Invoke("ToTitle", 2f);
        }
    }

    private void ToTitle()
    {
        SceneManager.LoadScene("01_Title");
    }
}
