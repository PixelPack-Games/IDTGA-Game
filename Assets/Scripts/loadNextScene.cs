using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class loadNextScene : NetworkBehaviour
{
    public Animator transition;
    public GameObject player;
    public float transitionTime = 1f;
    // Start is called before the first frame update

    public int LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        return SceneManager.GetActiveScene().buildIndex;
    }
    public IEnumerator LoadBattleScene(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
