using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class BattleTrigger : NetworkBehaviour
{
    public loadNextScene loadNextScene;
    public PlayerMovement player_Movement;
    void Start()
    {
        // Find the loadNextScene script in the scene
        loadNextScene = FindObjectOfType<loadNextScene>();
        player_Movement = GetComponent<PlayerMovement>();

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
            //make sure the player is inBattle now
            player_Movement.inBattle = true;
            //this gets the next scene from the build settings
            loadNextScene.LoadNextLevel();
        }
    }


}