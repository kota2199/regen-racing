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

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    private AudioClip countDownSE, goSE;

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
        audioSource.PlayOneShot(countDownSE);
        yield return StartCoroutine(CountDownAnimate("3"));
        audioSource.PlayOneShot(countDownSE);
        yield return StartCoroutine(CountDownAnimate("2"));
        audioSource.PlayOneShot(countDownSE);
        yield return StartCoroutine(CountDownAnimate("1"));
        countDownText.DOFade(endValue: 1f, duration: 0f);

        audioSource.PlayOneShot(goSE);
        countDownText.text = "Go!";

        isPlay = true;
        raceData.isPlay = true;
        panel.SetActive(false);

        yield return new WaitForSeconds(1f);
        var sequence = DOTween.Sequence();
        sequence.Append(countDownText.DOFade(endValue: 0f, duration: 1f));
        sequence.Join(countDownText.transform.DOScale(0, 1f));
        sequence.Join(countDownText.DOFade(endValue: 0f, duration: 1f));
        yield return sequence.Play().WaitForCompletion();
        countDownObj.SetActive(false);
    }

    private IEnumerator CountDownAnimate(string message)
    {
        int originalFontSize = countDownText.fontSize;
        countDownText.text = message;

        var sequence = DOTween.Sequence();
        sequence.Append(countDownText.DOFade(endValue: 1f, duration: 0f));
        sequence.Append(DOVirtual.Int(originalFontSize, countDownTextTargetFontSize, 1, value =>
        {
            countDownText.fontSize = value;
        }));
        sequence.Join(countDownText.DOFade(endValue: 0f, duration: 1f));
        yield return sequence.Play().WaitForCompletion();

        countDownText.fontSize = originalFontSize;
    }
}
