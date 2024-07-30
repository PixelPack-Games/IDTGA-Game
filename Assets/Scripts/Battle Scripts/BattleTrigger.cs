using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class BattleTrigger : NetworkBehaviour
{
    public BattleSystem BattleSystem;
    public loadNextScene loadNextScene;
    public PlayerInput player_Input;
    public Network network;
    public Vector3 OverworldLocation;
    void Start()
    {
        Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Initialize()
    {
            loadNextScene = FindObjectOfType<loadNextScene>();
            BattleSystem = FindObjectOfType<BattleSystem>();
            player_Input = GetComponent<PlayerInput>();
            // Find the loadNextScene script in the scene
            if (loadNextScene == null)
            {
                Debug.LogError("loadNextScene script not found in the scene.");
            }
        
    }

    /*void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!IsOwner) return;
        triggerBattle(other);
    }

    public void triggerBattle(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (IsOwner)
            {
                OverworldLocation = this.gameObject.transform.position;
                checkIfEnemy(other);
                PlayerData temp = new PlayerData()
                {
                    //sceneIndex = loadNextScene.LoadNextLevel()
                };
                if (IsServer || !Network.serverAuth)
                {
                    network.data.Value = temp;
                }
                else
                {
                    network.transmitDataServerRpc(temp);
                }
                BattleSystem.StartBattle(1);
            }
            else
            {
                BattleSystem.StartBattle(1);
            }
        }
    }

    public void checkIfEnemy(Collider2D other)
    {
        
        BattleManager.Instance.SetBattleData(this.gameObject, other.gameObject, OverworldLocation);
        //makes sure these objects can be referenced in other scenes
        //DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(other.gameObject);
        //make sure the player is inBattle now
        player_Input.inBattle = true;
        
        //this gets the next scene from the build settings
    }


}