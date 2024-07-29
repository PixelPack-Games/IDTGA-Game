using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PauseButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void pressPauseButton(){
        int clientId = (int)NetworkManager.Singleton.LocalClientId;
        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = playerGameObjects[clientId];
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        playerInput.pauseGame();
    }
    public void pressResumeButton(){
        int clientId = (int)NetworkManager.Singleton.LocalClientId;
        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject player = playerGameObjects[clientId];
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        playerInput.unpauseGame();
    }
}
