using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private RaceData raceDate;

    [SerializeField]
    private List<GameObject> Uis;

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
    private AudioClip decide;

    private void Start()
    {
        raceDate.UpdateCarPositionsByTime();

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
            Uis[i].transform.Find("t_Pos").GetComponent<Text>().text = raceDate.cars[i].Position.ToString() + MakeOrdinalNumber(raceDate.cars[i].Position.ToString());
            Uis[i].transform.Find("t_Time").GetComponent<Text>().text = FormatToMMSSSSS(raceDate.cars[i].time);
            Uis[i].transform.Find("t_Eco").GetComponent<Text>().text = ecoPoint[raceDate.cars[i].choiceIndex].ToString();
            Uis[i].transform.Find("t_Total").GetComponent<Text>().text = FormatToMMSSSSS(TotalTime(raceDate.cars[i].choiceIndex,raceDate.cars[i].time));
        }
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
        // 分を計算
        int minutes = (int)(totalSeconds / 60);
        // 秒を計算
        float seconds = totalSeconds % 60;

        // MM:SS.SSS形式の文字列を返す
        return $"{minutes}:{seconds:00.000}";
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

    private void Update()
    {
        if (Input.GetButtonDown("Fire_0") || Input.GetKeyDown(KeyCode.C))
        {
            GetComponent<AudioSource>().PlayOneShot(decide);
            fadeInOut.FadeOut();
            Invoke("ToTitle", 2f);
        }
    }

    private void ToTitle()
    {
        SceneManager.LoadScene("01_Title");
    }
}
