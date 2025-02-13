using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    private  VideoPlayer videoPlayer;

    [SerializeField]
    private string menuSceneName;

    [SerializeField]
    private ScreenFader screenFader;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }

    public void LoopPointReached(VideoPlayer vp)
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return StartCoroutine(screenFader.FadeOut());
        SceneManager.LoadScene(menuSceneName);

    }
}
