using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipExplainMovie : MonoBehaviour
{
    [SerializeField]
    private string cross;

    [SerializeField]
    private ScreenFader screenFader;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cross))
        {
            StartCoroutine(ToMenu());
        }
    }

    IEnumerator ToMenu()
    {
        yield return StartCoroutine(screenFader.FadeOut());
        SceneManager.LoadScene("03_Menu");
    }
}
