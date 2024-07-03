using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildhubSpawn : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        player = PlayerMovement.gameObject;
        player.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
