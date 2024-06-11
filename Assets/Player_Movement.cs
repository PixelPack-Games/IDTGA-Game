using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Rigidbody2D player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)){
            player.velocity = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow)){
            player.velocity = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow)){
            player.velocity = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            player.velocity = Vector2.down;
        }
        
        else {
            player.velocity = Vector2.zero;
        }
    }
}
