using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D player;
    public float movementSpeed;
    public SpriteRenderer sprite;
    public Animator animator;
    private float speed;
    public NetworkObject netObj;
    public bool inBattle;
  
    // Start is called before the first frame update
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if (netObj.IsSpawned == false)
        {
            netObj.Spawn();
        }
        DontDestroyOnLoad(this.gameObject);
        speed = movementSpeed * 100 * Time.fixedDeltaTime;
    }
    
    public void moveUp(){
        player.velocity = Vector2.up * speed;
        animator.SetFloat("Velocity", 1);
    }
    public void moveLeft(){
        player.velocity = Vector2.left * speed;
        animator.SetFloat("Velocity", 1);
        if(!sprite.flipX){
            sprite.flipX = true;
        }
    }
    public void moveDown(){
        player.velocity = Vector2.down * speed;
        animator.SetFloat("Velocity", 1);
    }
    public void moveRight(){
        player.velocity = Vector2.right * speed;
        animator.SetFloat("Velocity", 1);
        if(sprite.flipX){
            sprite.flipX = false;
        }
    }
    public void stopMovement(){
        player.velocity = Vector2.zero;
        animator.SetFloat("Velocity", 0);
    }
}