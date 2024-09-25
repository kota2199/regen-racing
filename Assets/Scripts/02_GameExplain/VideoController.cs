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
        SceneManager.LoadScene(menuSceneName);
    }
}
