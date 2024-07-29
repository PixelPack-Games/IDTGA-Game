using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public string pauseButtonName;
    private static GameObject pauseButton;
    void Start(){

    }
    public void activatePauseMenu(){
        findPauseButton();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        pauseButton.SetActive(false);
    }

    public void deactivatePauseMenu()
    {
        SceneManager.UnloadSceneAsync(sceneName);
        if (pauseButton == null)
        {
            findPauseButton();
        }
        pauseButton.SetActive(true);
    }

    private void findPauseButton()
    {
        GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].name.Equals(pauseButtonName))
            {
                continue;
            }

            pauseButton = objects[i];
        }
    }
}
