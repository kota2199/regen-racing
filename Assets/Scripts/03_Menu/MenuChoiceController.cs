using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuChoiceController : MonoBehaviour
{
    [Header("Future Choice")]
    public bool isSelecting;

    private bool isColorSelecting;

    public enum MenuMode
    {
        CarChoice,
        ChoiceCarColor
    }

    private MenuMode menuMode;

    [SerializeField]
    private GameObject choiceUI;

    [SerializeField]
    private GameObject[] choices;

    [SerializeField]
    private Vector3 markerPosOffset;

    [SerializeField]
    private Image explain;

    [SerializeField]
    private Sprite[] explainImage;

    [SerializeField]
    private int currentChoiceNum;

    [Space(20)]

    [Header("Color Choice")]
    [SerializeField]
    private GameObject colorChoiceUI;

    [SerializeField]
    private GameObject colorModelCar;

    private ColorSelectController selecter;

    [SerializeField]
    private string decisionButton, detailButton, cancelButton;

    [SerializeField]
    private bool selectRight = false, selectLeft = false;

    [SerializeField]
    private AudioSource audioSourceForGuide, audioSourceSE;

    [SerializeField]
    private AudioClip carChoice, colorChoice;

    [SerializeField]
    private AudioClip decision, choice;

    [SerializeField]
    private string raceSceneName;

    [SerializeField]
    private ScreenFader screenFader;

    [SerializeField]
    private RaceData raceData;

    private bool isDecision;

    void Awake()
    {
        selecter = GetComponent<ColorSelectController>();
        audioSourceForGuide = GetComponent<AudioSource>();
        isSelecting = true;
        isColorSelecting = false;
        menuMode = MenuMode.CarChoice;
        colorModelCar.SetActive(false);
        currentChoiceNum = 0;
        isDecision = false;
    }

    private void Start()
    {
        StartCoroutine(UpdateMenuMode(MenuMode.CarChoice, null));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDecision)
        {
            UpdateInput();
            RecieveInput();
        }
        
        UpdateUI();
    }
    private void RecieveInput()
    {
        if (Input.GetAxis("Horizontal Stick-L") > 0.05f && !selectRight)
        {
            selectRight = true;
            RecieveRightInput();
        }
        else if(Input.GetAxis("Horizontal Stick-L") < 0.05f)
        {
            selectRight = false;
        }

        if (Input.GetAxis("Horizontal Stick-L") < -0.05f && !selectLeft)
        {
            selectLeft = true;
            RecieveLeftInput();
        }
        else if (Input.GetAxis("Horizontal Stick-L") > -0.05f)
        {
            selectLeft = false;
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown(decisionButton))
        {
            if (isSelecting)
            {
                raceData.playerChoiceIndex = currentChoiceNum;
                StartCoroutine(UpdateMenuMode(MenuMode.ChoiceCarColor, choices[currentChoiceNum].transform));
            }
            else if (isColorSelecting)
            {
                StartCoroutine(ToRaceScene());
            }
        }

        if(Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cancelButton))
        {
            if (isColorSelecting)
            {
                StartCoroutine(UpdateMenuMode(MenuMode.CarChoice, null));
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RecieveLeftInput();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RecieveRightInput();
        }
    }

    private void RecieveRightInput()
    {
        if (isSelecting && currentChoiceNum < choices.Length - 1)
        {
            currentChoiceNum++;
            UpdateOptions();
            audioSourceSE.PlayOneShot(choice);
        }
    }

    private void RecieveLeftInput()
    {
        if (isSelecting && currentChoiceNum > 0)
        {
            currentChoiceNum--;
            UpdateOptions();
            audioSourceSE.PlayOneShot(choice);
        }
    }

    private void UpdateUI()
    {
        choiceUI.SetActive(isSelecting);

        colorChoiceUI.SetActive(isColorSelecting);
        colorModelCar.SetActive(isColorSelecting);
    }

    private void UpdateOptions()
    {
        for(int i = 0; i < choices.Length; i++)
        {
            if(i == currentChoiceNum)
            {
                choices[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                StartCoroutine(ImageAnimationForFutureChoice(choices[i].transform));
                StartCoroutine(ImageAnimationForFutureExplain(explainImage[i]));
            }
            else
            {
                choices[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            }
        }
    }

    private IEnumerator UpdateMenuMode(MenuMode nextMenuMode, Transform animTarget)
    {
        menuMode = nextMenuMode;
        audioSourceSE.PlayOneShot(decision);

        isDecision = true;

        if(animTarget != null)
        {
            yield return StartCoroutine(ImageAnimationForSceneChange(animTarget));
        }

        switch (menuMode)
        {
            case MenuMode.CarChoice:

                isSelecting = true;
                isColorSelecting = false;
                isDecision = false;
                selecter.isColorSelecting = isColorSelecting;

                UpdateOptions();

                if (audioSourceForGuide.isPlaying)
                {
                    audioSourceForGuide.Stop();
                    StartCoroutine(PlayGuideVoice(carChoice));
                }

                break;

            case MenuMode.ChoiceCarColor:

                isSelecting = false;
                isColorSelecting = true;
                isDecision = false;
                selecter.isColorSelecting = isColorSelecting;

                audioSourceForGuide.Stop();
                StartCoroutine(PlayGuideVoice(colorChoice));

                break;

            default:
                break;
        }
    }

    private IEnumerator ToRaceScene()
    {
        isDecision = true;
        audioSourceForGuide.Stop();
        audioSourceSE.PlayOneShot(decision);
        yield return StartCoroutine(screenFader.FadeOut());
        SceneManager.LoadScene(raceSceneName);
    }

    private IEnumerator PlayGuideVoice(AudioClip clip)
    {
        audioSourceForGuide.Stop();
        yield return new WaitForSeconds(0.5f);
        audioSourceForGuide.PlayOneShot(clip);
    }
    private IEnumerator ImageAnimationForSceneChange(Transform animTarget)
    {
        float originalScale = animTarget.transform.localScale.x;
        var sequence = DOTween.Sequence();
        sequence.Append(animTarget.DOScale(originalScale * 0.9f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(animTarget.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Join(animTarget.gameObject.GetComponent<Image>().DOFade(endValue: 0f, duration: 0.1f));
        sequence.Append(animTarget.DOScale(originalScale, 0f));
        yield return sequence.Play().WaitForCompletion();
    }
    private IEnumerator ImageAnimationForFutureChoice(Transform animTarget)
    {
        float originalScale = animTarget.transform.localScale.x;
        var sequence = DOTween.Sequence();
        sequence.Append(animTarget.DOScale(originalScale * 0.9f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(animTarget.DOScale(originalScale * 1f, 0.2f).SetEase(Ease.OutBack));
        yield return sequence.Play().WaitForCompletion();
    }
    private IEnumerator ImageAnimationForFutureExplain(Sprite nextExplain)
    {
        yield return explain.DOFade(endValue: 0f, duration: 0.1f).WaitForCompletion();
        explain.sprite = nextExplain;
        yield return new WaitForSeconds(0.1f);
        explain.DOFade(endValue: 1f, duration: 0.1f);
    }
}
