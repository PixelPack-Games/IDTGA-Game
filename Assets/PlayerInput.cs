using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Network network;
    public PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    public NetworkObject networkObject;
    public bool inBattle;
    public bool isPaused;

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
}