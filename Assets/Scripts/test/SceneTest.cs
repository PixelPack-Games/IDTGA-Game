using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string prevScene;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(prevScene, LoadSceneMode.Single);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
