using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public Network network;
    public PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    public PauseMenu pauseMenu;
    public Animator animator;

    public NetworkObject networkObject;
    public bool inBattle;
    public bool isPaused;
    private GameObject pauseButton;
    GameObject player;
    Rigidbody2D rb;
    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        if (networkObject.IsSpawned == false)
        {
            networkObject.Spawn();
        }
        DontDestroyOnLoad(this.gameObject);
        player = this.GameObject();
        rb = player.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;
        if(velocity.x != 0 || velocity.y != 0){
            animator.SetFloat("Velocity", 1);
        }
        else{
            animator.SetFloat("Velocity", 0);
        }
        

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