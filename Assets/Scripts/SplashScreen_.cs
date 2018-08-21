using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen_ : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.SetInt("Voice", 1);
        PlayerPrefs.SetInt("Music", 1);

        StartCoroutine(PlaySplashVideo("opening_2.mp4"));
    }

    IEnumerator PlaySplashVideo(string videoName)
    {
        Handheld.PlayFullScreenMovie(videoName, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
        yield return new WaitForEndOfFrame();
        LoadManager.level = "Title";
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadLevel");
    }
}
