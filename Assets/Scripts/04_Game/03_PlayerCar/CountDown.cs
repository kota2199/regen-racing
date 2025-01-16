using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public static CountDown instance;

    [SerializeField]
    private RaceData raceData;

    public GameObject countDownObj, panel;

    public Text countDownText;

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
    }
    void Start()
    {
        StartCoroutine("CountingDown");   
    }

    private IEnumerator CountingDown()
    {
        isPlay = false;
        yield return new WaitForSeconds(2);
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "Go!";
        isPlay = true;
        raceData.isPlay = true;
        panel.SetActive(false);
        yield return new WaitForSeconds(1);
        countDownObj.SetActive(false);
    }
}
