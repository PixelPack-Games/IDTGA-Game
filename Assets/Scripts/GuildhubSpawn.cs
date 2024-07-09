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

        if (PlayerMovement == null)
        {
            Debug.Log("Player not found");
            return;
        }

        player = PlayerMovement.gameObject;
        player.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            return;
        }

        PlayerMovement = FindObjectOfType<PlayerMovement>();

        if (PlayerMovement == null)
        {
            Debug.Log("Player not found");
            return;
        }

        player = PlayerMovement.gameObject;
        player.transform.position = this.transform.position;
    }
}
