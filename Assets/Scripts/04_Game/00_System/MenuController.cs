using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    private bool onMenu = false;

    public bool isGameOver = false;

    [SerializeField]
    private GameObject playerCar;

    [SerializeField]
    private GameObject menuUI, gameOverUI;

    private SceneTransition sceneController;

    private ReplaceController replaceController;

    private LapCounter lapCounter;

    [SerializeField]
    private string menuButton, retryButton, replaceButton, quitButton;

    // Start is called before the first frame update
    void Awake()
    {
        onMenu = false;
        menuUI.SetActive(onMenu);
        replaceController = playerCar.GetComponent<ReplaceController>();
        lapCounter = playerCar.GetComponent<LapCounter>();
        sceneController = GetComponent<SceneTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(menuButton) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onMenu)
            {
                onMenu = true;
                StartCoroutine(DisplayMenuPanelAnimation());
            }
            else
            {
                onMenu = false;
                StartCoroutine(HideMenuPanelAnimation());
            }
        }

        if (onMenu || lapCounter.isFinished || isGameOver)
        {
            if (Input.GetButtonDown(quitButton) || Input.GetKeyDown(KeyCode.C))
            {
                ToMenu();
            }
            if (Input.GetButtonDown(replaceButton) || Input.GetKeyDown(KeyCode.D))
            {
                Replace();
            }
            if (Input.GetButtonDown(retryButton) || Input.GetKeyDown(KeyCode.B))
            {
                Retry();
            }
        }

        gameOverUI.SetActive(isGameOver);
    }

    public void ToMenu()
    {
        sceneController.ToHome();
    }
    public void Resume()
    {
        onMenu = false;
        StartCoroutine(DisplayMenuPanelAnimation());
    }
    public void Replace()
    {
        replaceController.Replace();
        onMenu = false;
        StartCoroutine(DisplayMenuPanelAnimation());
    }
    public void Retry()
    {
        sceneController.Retry();
    }

    IEnumerator DisplayMenuPanelAnimation()
    {
        menuUI.gameObject.SetActive(onMenu);
        yield return menuUI.gameObject.GetComponent<CanvasGroup>().DOFade(endValue: 1f, duration: 0.2f).WaitForCompletion();
    }
    IEnumerator HideMenuPanelAnimation()
    {
        yield return menuUI.gameObject.GetComponent<CanvasGroup>().DOFade(endValue: 0f, duration: 0.2f).WaitForCompletion();
        menuUI.gameObject.SetActive(onMenu);
    }
}
