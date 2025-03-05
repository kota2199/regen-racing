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

    [SerializeField]
    private string menuButton, retryButton, replaceButton, quitButton;

    private int selectIndex = 0;

    [SerializeField]
    private Image[] menuOptions;

    [SerializeField]
    private Sprite[] optionsImages;
    [SerializeField]
    private Sprite[] selectedImages;

    [SerializeField]
    private Image preview;

    [SerializeField]
    private Sprite[] previewImages;

    [SerializeField]
    private AudioSource seAudioSource;

    [SerializeField]
    private AudioClip choice, decision;

    // Start is called before the first frame update
    void Awake()
    {
        onMenu = false;
        menuUI.SetActive(onMenu);
        replaceController = playerCar.GetComponent<ReplaceController>();
        sceneController = GetComponent<SceneTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SelectIndex is " + selectIndex);

        if (Input.GetButtonDown(menuButton) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onMenu)
            {
                onMenu = true;
                UpdateMenuOptionImage();
                StartCoroutine(DisplayMenuPanelAnimation());
            }
            else
            {
                onMenu = false;
                StartCoroutine(HideMenuPanelAnimation());
            }
        }

        if (onMenu)
        {
            if (Input.GetAxis("CrossKey_Vertical") > 0 || Input.GetKeyDown(KeyCode.UpArrow) && selectIndex < menuOptions.Length - 1)
            {
                selectIndex++;
                UpdateMenuOptionImage();
            }
            if (Input.GetAxis("CrossKey_Vertical") < 0 || Input.GetKeyDown(KeyCode.DownArrow) && selectIndex > 0)
            {
                selectIndex--;
                UpdateMenuOptionImage();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectOption();
            }
        }

        gameOverUI.SetActive(isGameOver);
    }

    private void UpdateMenuOptionImage()
    {
        for(int i = 0; i < menuOptions.Length; i++)
        {
            if(i == selectIndex)
            {
                menuOptions[i].sprite = selectedImages[i];
                preview.sprite = previewImages[i];
            }
            else
            {
                menuOptions[i].sprite = optionsImages[i];
            }
        }

        seAudioSource.PlayOneShot(choice);
    }

    private void SelectOption()
    {
        seAudioSource.PlayOneShot(decision);

        switch (selectIndex)
        {
            case 0:
                Replace();
                break;
            case 1:
                Retry();
                break;
            case 2:
                ToMenu();
                break;
            case 3:
                onMenu = false;
                StartCoroutine(HideMenuPanelAnimation());
                break;
            default:
                Debug.LogError("OutOfIndex");
                break;
        }
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
