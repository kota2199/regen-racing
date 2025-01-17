using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RaceResultViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject gameUi,finishUi;
        
    [SerializeField]
    private RectTransform finishPanel;

    [SerializeField]
    private Text t_Position, t_OrdinalNum, t_Time;

    private RacePositionManager racePositionManager;

    [SerializeField]
    private Vector3 defaultPos, targetPos;

    [SerializeField]
    private float animTime;

    private void Awake()
    {
        racePositionManager = GetComponent<RacePositionManager>();
    }

    private void Start()
    {
        finishPanel.anchoredPosition = defaultPos;
    }
    public void DisplayResult(float totalTime)
    {
        int pos = racePositionManager.GetPosition("Player");
        gameUi.SetActive(false);
        finishUi.SetActive(true);

        t_Position.text = pos.ToString();
        t_OrdinalNum.text = MakeOrdinalNumber(pos.ToString());
        t_Time.text = FormatToMMSSSSS(totalTime);

        finishPanel.DOAnchorPos(targetPos, animTime).SetEase(Ease.OutExpo);
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
        // �����v�Z
        int minutes = (int)(totalSeconds / 60);
        // �b���v�Z
        float seconds = totalSeconds % 60;

        // MM:SS.SSS�`���̕������Ԃ�
        return $"{minutes}:{seconds:00.000}";
    }
}
