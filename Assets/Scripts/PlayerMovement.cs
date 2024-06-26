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
    // Start is called before the first frame update
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if(netObj.IsSpawned == false)
        {
            netObj.Spawn();
        }
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = movementSpeed * 100 * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inBattle)
        {
            if (Input.GetKey(KeyCode.A))
            {
                player.velocity = Vector2.left * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player.velocity = Vector2.right * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                player.velocity = Vector2.up * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                player.velocity = Vector2.down * movementSpeed;
            }

            else
            {
                player.velocity = Vector2.zero;
            }
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
        
    }
}
