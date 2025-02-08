using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void StartMainGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void StartEndingScene()
    {
        StartCoroutine(StartEndingSceneWithDelay());
    }

    private IEnumerator StartEndingSceneWithDelay()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("EndingScene");
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
