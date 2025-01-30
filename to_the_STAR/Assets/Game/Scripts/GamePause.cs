using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public static bool isGamePaused = false; // Indicate whether game is Paused or not
    public GameObject pauseMenuUI;
    public AudioSource music;
    private AudioLowPassFilter musicFilter;    

    void Start()
    {
        pauseMenuUI.SetActive(false);

        musicFilter = music.GetComponent<AudioLowPassFilter>();
        if(musicFilter == null)
            musicFilter = music.gameObject.AddComponent<AudioLowPassFilter>();
        musicFilter.cutoffFrequency = 22000f;   // Default value (original music)
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused) Resume();
            else Pause();
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        musicFilter.cutoffFrequency = 22000f;
        Debug.Log("Resumed");
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        musicFilter.cutoffFrequency = 800f;
        Debug.Log("Paused");
    }
}
