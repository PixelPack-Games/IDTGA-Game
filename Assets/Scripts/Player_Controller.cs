using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D player;
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKey(KeyCode.A)){
            player.velocity = Vector2.left * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D)){
            player.velocity = Vector2.right * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.W)){
            player.velocity = Vector2.up * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S)){
            player.velocity = Vector2.down * movementSpeed;
        }
        
        else {
            player.velocity = Vector2.zero;
        }
    }
}
