using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    public static CountDown instance;

    [SerializeField]
    private RaceData raceData;

    public GameObject countDownObj, panel;

    public Text countDownText;

    [SerializeField]
    private int countDownTextTargetFontSize;

    public bool isPlay;
    //すべてRaceDataのisPlayに置き換える

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        raceData.isPlay = false;
    }
    void Start()
    {
        StartCoroutine("CountingDown");   
    }

    private IEnumerator CountingDown()
    {
        isPlay = false;
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(CountDownAnimate("3"));
        yield return StartCoroutine(CountDownAnimate("2"));
        yield return StartCoroutine(CountDownAnimate("1"));

        //yield return new WaitForSeconds(2);
        //countDownText.text = "3";
        //yield return new WaitForSeconds(1);
        //countDownText.text = "2";
        //yield return new WaitForSeconds(1);
        //countDownText.text = "1";
        //yield return new WaitForSeconds(1);
        countDownText.text = "Go!";
        isPlay = true;
        raceData.isPlay = true;
        panel.SetActive(false);
        yield return new WaitForSeconds(1);
        countDownObj.SetActive(false);
    }

    private IEnumerator CountDownAnimate(string message)
    {
        int originalFontSize = countDownText.fontSize;
        countDownText.text = message;

        var sequence = DOTween.Sequence();
        sequence.Append(countDownText.DOFade(endValue: 1f, duration: 1f));
        sequence.Append(DOVirtual.Int(originalFontSize, countDownTextTargetFontSize, 1, value =>
        {
            countDownText.fontSize = value;
        }));
        sequence.Join(countDownText.DOFade(endValue: 0f, duration: 1f));
        yield return sequence.Play().WaitForCompletion();

        countDownText.fontSize = originalFontSize;
    }
}
