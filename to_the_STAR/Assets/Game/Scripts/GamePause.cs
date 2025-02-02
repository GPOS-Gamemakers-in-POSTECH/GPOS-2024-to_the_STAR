using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GamePause : MonoBehaviour
{
    public static bool isGamePaused = false; // Indicate whether game is Paused or not
    public GameObject pauseMenuUI;

    public AudioMixer mixer;

    public Button resumeButton;
    public Button titleButton;
    public Button exitButton;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        mixer.SetFloat("Master_Cutoff", 22000f); // Default value (original music)

        if(resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if(titleButton != null)
            titleButton.onClick.AddListener(Title);

        if(exitButton != null)
            exitButton.onClick.AddListener(Exit);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused) Resume();
            else Pause();
        }
    }

    // Function that resumes the game
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        isGamePaused = false;

        Time.timeScale = 1f;

        mixer.SetFloat("Master_Cutoff", 22000f);
    }

    // Function that pauses the game
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        isGamePaused = true;

        Time.timeScale = 0f;

        mixer.SetFloat("Master_Cutoff", 800f);
    }

    // Function that moves to the title scene
    private void Title()
    {    
        Resume();
        SceneManager.LoadScene("TitleScene");
    }

    // Function that exits the game
    private void Exit()
    {
        PlayerPrefs.DeleteAll(); // refresh PlayerPrefs (must be removed in the final version)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
