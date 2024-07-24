using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D player;
    public float movementSpeed;
    public NetworkObject netObj;
    public bool inBattle;
    float speed;
    private SpriteRenderer sprite;
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
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
        // Update is called once per frame
        void Update()
        {
            if (!inBattle)
        {
            if (Input.GetKey(KeyCode.A))
            {
                player.velocity = Vector2.left * speed;
                if(!sprite.flipX){
                    sprite.flipX = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player.velocity = Vector2.right * speed;
                if(sprite.flipX){
                    sprite.flipX = false;
                }
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