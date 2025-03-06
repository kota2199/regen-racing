using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private string circle, triangle, square, cross;

    public enum Mode
    {
        Title, Choice
    }

    private Mode currentMode;

    [SerializeField]
    private GameObject titleCanvas, choiceCanvas;

    [SerializeField]
    private string explainSceneName, gameSceneName;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip decision, choice;

    [SerializeField]
    private ScreenFader screenFader;

    [SerializeField]
    private Transform start, explain, menu;

    private Transform startImageDefaultTransform;

    [SerializeField]
    private RaceData raceData;

    private bool isDecision;

    private bool selectRight = false, selectLeft = false;

    private int choiceIndex = 0;

    [SerializeField]
    private GameObject[] choiceImages;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        raceData.Initialize();
        isDecision = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeMode(Mode.Title, null));
        startImageDefaultTransform = start.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDecision)
        {
            RecieveInput();
        }
    }

    private void RecieveInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown(circle))
        {
            if(currentMode == Mode.Title)
            {
                StartCoroutine(ChangeMode(Mode.Choice, start));
            }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cross))
        {
            if (currentMode == Mode.Choice)
            {
                StartCoroutine(ChangeMode(Mode.Title, null));
            }
        }

        if(currentMode == Mode.Choice)
        {
            if ((Input.GetAxis("Horizontal Stick-L") > 0.05f || Input.GetKey(KeyCode.RightArrow)) && !selectRight && choiceIndex < 1)
            {
                selectRight = true;
                SelectMode(1);
            }
            else if (Input.GetAxis("Horizontal Stick-L") < 0.05f && !Input.GetKey(KeyCode.RightArrow))
            {
                selectRight = false;
            }

            if ((Input.GetAxis("Horizontal Stick-L") < -0.05f || Input.GetKey(KeyCode.LeftArrow)) && !selectLeft && choiceIndex > 0)
            {
                selectLeft = true;
                SelectMode(-1);
            }
            else if (Input.GetAxis("Horizontal Stick-L") > -0.05f && !Input.GetKey(KeyCode.LeftArrow))
            {
                selectLeft = false;
            }

            if(Input.GetButtonDown("Circle") || Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(ToNextScene());
            }
        }
    }

    private void SelectMode(int a)
    {
        choiceIndex += a;
        for(int i = 0; i < choiceImages.Length; i++)
        {
            if(i == choiceIndex)
            {
                StartCoroutine(MenuSelectAnim(choiceImages[i].transform));
                Debug.Log("Animate");
            }
            else
            {
                choiceImages[i].transform.localScale = new Vector3(1, 1, 1);
                Debug.Log("Size Reset");
            }
        }

    }

    private IEnumerator ChangeMode(Mode mode, Transform animTarget)
    {
        audioSource.PlayOneShot(decision);

        if(animTarget != null)
        {
            yield return StartCoroutine(ImageAnimation(animTarget));
        }

        switch (mode)
        {
            case Mode.Title:
                currentMode = Mode.Title;
                titleCanvas.SetActive(true);
                choiceCanvas.SetActive(false);
                start.transform.position = startImageDefaultTransform.position;
                start.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                start.gameObject.GetComponent<Image>().DOFade(endValue: 1f, duration: 0f);
                break;

            case Mode.Choice:
                currentMode = Mode.Choice;
                titleCanvas.SetActive(false);
                choiceCanvas.SetActive(true);
                SelectMode(0);
                break;

            default:
                break;
        }
    }

    private IEnumerator ToNextScene()
    {
        isDecision = true;

        audioSource.PlayOneShot(decision);

        yield return StartCoroutine(MenuAnimToNext(choiceImages[choiceIndex].transform));

        yield return StartCoroutine(screenFader.FadeOut());

        switch (choiceIndex)
        {
            case 0:
                SceneManager.LoadScene(explainSceneName);
                break;
            case 1:
                SceneManager.LoadScene(gameSceneName);
                break;
        }
    }

    private IEnumerator ImageAnimation(Transform animTarget)
    {
        float originalScale = animTarget.transform.localScale.x;
        var sequence = DOTween.Sequence();
        sequence.Append(animTarget.DOScale(originalScale * 0.9f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(animTarget.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Join(animTarget.gameObject.GetComponent<Image>().DOFade(endValue: 0f, duration: 0.1f));
        yield return sequence.Play().WaitForCompletion();
    }

    private IEnumerator MenuSelectAnim(Transform animTarget)
    {
        float originalScale = animTarget.transform.localScale.x;
        var sequence = DOTween.Sequence();
        sequence.Append(animTarget.DOScale(originalScale * 0.9f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(animTarget.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack));
        yield return sequence.Play().WaitForCompletion();
    }

    private IEnumerator MenuAnimToNext(Transform animTarget)
    {
        float originalScale = animTarget.transform.localScale.x;
        var sequence = DOTween.Sequence();
        sequence.Append(animTarget.DOScale(originalScale * 0.9f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(animTarget.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Join(animTarget.gameObject.GetComponent<Image>().DOFade(endValue: 0f, duration: 0.1f));
        sequence.Append(animTarget.DOScale(originalScale, 0f));
        yield return sequence.Play().WaitForCompletion();
    }
}
