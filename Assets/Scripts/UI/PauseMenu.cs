using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void onPauseClick()
    {
        if (sceneName == default)
        {
            Debug.LogError("No scene name given in the Editor");
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void onResumeClick()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
