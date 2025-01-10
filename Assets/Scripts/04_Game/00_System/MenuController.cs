using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake()
    {
        onMenu = false;
        replaceController = playerCar.GetComponent<ReplaceController>();
        lapCounter = playerCar.GetComponent<LapCounter>();
        sceneController = GetComponent<SceneTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire_Select") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!onMenu)
            {
                onMenu = true;
            }
            else
            {
                onMenu = false;
            }
        }

        if (onMenu || lapCounter.isFinished || isGameOver)
        {
            if (Input.GetButtonDown("Fire_0") || Input.GetKeyDown(KeyCode.C))
            {
                ToMenu();
            }
            if (Input.GetButtonDown("Fire_1") || Input.GetKeyDown(KeyCode.D))
            {
                Replace();
            }
            if (Input.GetButtonDown("Fire_3") || Input.GetKeyDown(KeyCode.B))
            {
                Retry();
            }
        }

        menuUI.SetActive(onMenu);
        gameOverUI.SetActive(isGameOver);
    }

    public void ToMenu()
    {
        sceneController.ToHome();
    }
    public void Resume()
    {
        onMenu = false;
    }
    public void Replace()
    {
        replaceController.Replace();
        onMenu = false;
    }
    public void Retry()
    {
        sceneController.Retry();
    }
}
