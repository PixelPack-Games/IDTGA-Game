using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class BattleTrigger : NetworkBehaviour
{
    public loadNextScene loadNextScene;

    void Start()
    {
        // Find the loadNextScene script in the scene
        loadNextScene = FindObjectOfType<loadNextScene>();

        if (loadNextScene == null)
        {
            Debug.LogError("loadNextScene script not found in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!IsOwner) return;
        if (other.CompareTag("Enemy"))
        {
            BattleManager.Instance.SetBattleData(this.gameObject, other.gameObject);
            //makes sure these objects can be referenced in other scenes
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(other.gameObject);
            //this gets the next scene from the build settings
            loadNextScene.LoadNextLevel();
        }
    }


}