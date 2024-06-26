using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D player;
    public float movementSpeed;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = movementSpeed * 100 * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKey(KeyCode.A)){
            player.velocity = Vector2.left * speed;
        }
        else if (Input.GetKey(KeyCode.D)){
            player.velocity = Vector2.right * speed;
        }
        else if (Input.GetKey(KeyCode.W)){
            player.velocity = Vector2.up * speed;
        }
        else if (Input.GetKey(KeyCode.S)){
            player.velocity = Vector2.down * speed;
        }
        
        else {
            player.velocity = Vector2.zero;
        }
    }
}
