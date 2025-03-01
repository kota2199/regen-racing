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
            else if(currentMode == Mode.Choice)
            {
                StartCoroutine(ToNextScene(explainSceneName, decision, explain));
            }
            audioSource.PlayOneShot(decision);
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown(triangle))
        {
            if(currentMode == Mode.Choice)
            {
                StartCoroutine(ToNextScene(gameSceneName, decision, menu));
            }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cross))
        {
            if (currentMode == Mode.Choice)
            {
                StartCoroutine(ChangeMode(Mode.Title, null));
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
                break;

            default:
                break;
        }
    }

    private IEnumerator ToNextScene(string sceneName, AudioClip clip, Transform animTarget)
    {
        isDecision = true;

        audioSource.PlayOneShot(clip);

        if (animTarget != null)
        {
            yield return StartCoroutine(ImageAnimation(animTarget));
        }

        yield return StartCoroutine(screenFader.FadeOut());
        SceneManager.LoadScene(sceneName);
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
}
