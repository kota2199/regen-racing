using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipExplainMovie : MonoBehaviour
{
    [SerializeField]
    private string cross;

    [SerializeField]
    private FadeInOut fadeInOut;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(cross))
        {
            fadeInOut.FadeOut();
            Invoke("ToMenu", 1f);
        }
    }

    void ToMenu()
    {
        SceneManager.LoadScene("03_Menu");
    }
}
