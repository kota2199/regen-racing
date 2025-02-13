using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private FadeInOut fadeInOut;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMode(Mode.Title);
    }

    // Update is called once per frame
    void Update()
    {
        RecieveInput();
    }

    private void RecieveInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown(circle))
        {
            if(currentMode == Mode.Title)
            {
                ChangeMode(Mode.Choice);
            }
            else if(currentMode == Mode.Choice)
            {
                SceneManager.LoadScene(explainSceneName);
                StartCoroutine(ToNextScene(explainSceneName, decision));
            }
            audioSource.PlayOneShot(decision);
        }

        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown(triangle))
        {
            if(currentMode == Mode.Choice)
            {
                StartCoroutine(ToNextScene(gameSceneName, decision));
            }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cross))
        {
            if (currentMode == Mode.Choice)
            {
                ChangeMode(Mode.Title);
            }
        }
    }

    private void ChangeMode(Mode mode)
    {
        audioSource.PlayOneShot(decision);
        switch (mode)
        {
            case Mode.Title:
                currentMode = Mode.Title;
                titleCanvas.SetActive(true);
                choiceCanvas.SetActive(false);
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

    private IEnumerator ToNextScene(string sceneName, AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        fadeInOut.FadeOut();
        yield return new WaitForSeconds(clip.length);
        SceneManager.LoadScene(sceneName);
    }
}
