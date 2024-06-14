using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class jyj_playerBehavior : NetworkBehaviour
{
    //[SerializeField] private string message;
    [SerializeField] private float speed;
    public bool turnOff = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log(message);
        //  }

        //transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed, Space.World);
        //movePlayer(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0));
    }

    public void movePlayer(Vector3 translation)
    {
        transform.Translate(translation * speed);
    }
}
