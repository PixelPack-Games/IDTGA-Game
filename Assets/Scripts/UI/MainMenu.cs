using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void onClick()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
