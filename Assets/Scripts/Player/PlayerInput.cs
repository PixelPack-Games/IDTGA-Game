using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public Network network;
    public PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    public PauseMenu pauseMenu;
    public NetworkObject networkObject;
    public bool inBattle;
    public bool isPaused;
    private GameObject pauseButton;

    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject.IsSpawned == false)
        {
            networkObject.Spawn();
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(network.IsOwner){
            //Pause Menu Inputs
            if (Input.GetKeyDown(KeyCode.Escape)){
                if(!isPaused){
                    pauseGame();
                    return;
                }
                if(isPaused){
                    unpauseGame();
                    return;
                }
            }
            if (!inBattle && !isPaused){
                //Inventory Inputs
                if (Input.GetKeyDown(KeyCode.I)){
                    playerInventory.InventoryScreen.SetActive(!playerInventory.InventoryScreen.activeSelf);
                }
                if (Input.GetKeyDown(KeyCode.Space)){
                    playerInventory.inventory.Save();
                }
                if (Input.GetKeyDown(KeyCode.L)){
                    playerInventory.inventory.Load();
                }

                //Movement Inputs
                if (Input.GetKey(KeyCode.W)){
                    playerMovement.moveUp();
                }
                else if (Input.GetKey(KeyCode.A)){
                    playerMovement.moveLeft();
                }
                else if (Input.GetKey(KeyCode.S)){
                    playerMovement.moveDown();
                }
                else if (Input.GetKey(KeyCode.D)){
                    playerMovement.moveRight();
                }
                else{
                    playerMovement.stopMovement();
                }
            }
            else{
                playerMovement.stopMovement();
            }
        }
    }
    public void pauseGame(){
        pauseMenu.activatePauseMenu();
        isPaused = true;
    }
    public void unpauseGame(){
        pauseMenu.deactivatePauseMenu();
        isPaused = false;
    }
}