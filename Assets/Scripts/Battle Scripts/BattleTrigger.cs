using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class BattleTrigger : NetworkBehaviour
{
    public loadNextScene loadNextScene;
    public PlayerMovement player_Movement;
    public Network network;
    void Start()
    {
        loadNextScene = FindObjectOfType<loadNextScene>();
        player_Movement = GetComponent<PlayerMovement>();
        // Find the loadNextScene script in the scene
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
            if (IsOwner)
            {
                checkIfEnemy(other);
                PlayerData temp = new PlayerData()
                {
                    sceneIndex = loadNextScene.LoadNextLevel()
                };
                if (IsServer || !network.serverAuth)
                {
                   network.data.Value = temp;
                }
                else
                {
                    network.transmitDataServerRpc(temp);
                }
            }
            else
            {
                StartCoroutine(loadNextScene.LoadBattleScene(network.data.Value.sceneIndex));
            }
        }
    }

    public void checkIfEnemy(Collider2D other)
    {
        BattleManager.Instance.SetBattleData(this.gameObject, other.gameObject);
        //makes sure these objects can be referenced in other scenes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(other.gameObject);
        //make sure the player is inBattle now
        player_Movement.inBattle = true;
        //this gets the next scene from the build settings
    }


}