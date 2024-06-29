using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D player;
    public float movementSpeed;
    public NetworkObject netObj;
    public bool inBattle;
    float speed;
    private Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if (netObj.IsSpawned == false)
        {
            netObj.Spawn();
        }

        entity = new Rogue("Amir", "player1", this.gameObject, 10, 2, 1, 5, 10);
        Rogue player = (Rogue)entity;
        Debug.Log(player.getName() + " created with " + player.getCurrHealth() + " health");
        LinkedList<Entity> weapons = player.getWeapons();
        weapons.iterate();

        speed = movementSpeed * 100 * Time.fixedDeltaTime;
    }
        // Update is called once per frame
        void Update()
        {

        //THIS IF STATEMENT IS FOR WHEN IN BATTLE
        if (!inBattle)
        {
            if (Input.GetKey(KeyCode.A))
            {
                player.velocity = Vector2.left * speed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player.velocity = Vector2.right * speed;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                player.velocity = Vector2.up * speed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                player.velocity = Vector2.down * speed;
            }
            else
            {
                player.velocity = Vector2.zero;
            }
        }
        else
        {
            player.velocity = Vector2.zero;
        }
                
          
        }
}