using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

public class Player_Movement_Only : MonoBehaviour
{
    public float movementSpeed;
    public bool inBattle;
    private Rigidbody2D player;
    private SpriteRenderer sprite;
    private Animator animator;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        speed = movementSpeed * 100 * Time.fixedDeltaTime;
        
    }
        // Update is called once per frame
        void Update()
        {
        if (!inBattle)
        {
            if (Input.GetKey(KeyCode.A))
            {
                player.velocity = Vector2.left * speed;
                animator.SetFloat("Velocity", 1);
                if(!sprite.flipX){
                    sprite.flipX = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player.velocity = Vector2.right * speed;
                animator.SetFloat("Velocity", 1);
                if(sprite.flipX){
                    sprite.flipX = false;
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                player.velocity = Vector2.up * speed;
                animator.SetFloat("Velocity", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                player.velocity = Vector2.down * speed;
                animator.SetFloat("Velocity", 1);
            }
            else
            {
                player.velocity = Vector2.zero;
                animator.SetFloat("Velocity", 0);
            }
        }
        else
        {
            player.velocity = Vector2.zero;
        }
    }
}