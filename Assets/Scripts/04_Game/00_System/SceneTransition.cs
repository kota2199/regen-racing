using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private string sceneName;

    private FadeInOut fadeController;

    [SerializeField]
    private string title, race;

    void Awake()
    {
        fadeController = GetComponent<FadeInOut>();
    }

    public void ToHome()
    {
        sceneName = title;
        fadeController.FadeOut();
        Invoke("SceneTrans",2);
    }
    public void ToRace()
    {
        sceneName = race;
        fadeController.FadeOut();
        Invoke("SceneTrans", 2);
    }
    public void Retry()
    {
        sceneName = SceneManager.GetActiveScene().name;
        fadeController.FadeOut();
        Invoke("SceneTrans", 2);
    }

    public void SceneTrans()
    {
        SceneManager.LoadScene(sceneName);
    }
}
