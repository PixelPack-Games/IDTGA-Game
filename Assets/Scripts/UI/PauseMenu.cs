using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private static GameObject pauseButton;
    public void onPauseClick()
    {
        if (sceneName == default)
        {
            Debug.LogError("No scene name given in the Editor");
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        if (pauseButton == null)
        {
            pauseButton = gameObject;
        }

        gameObject.SetActive(false);
    }

    public void onResumeClick()
    {
        SceneManager.UnloadSceneAsync(sceneName);

        if (pauseButton == null)
        {
            return;
        }

        pauseButton.SetActive(true);
    }
}
