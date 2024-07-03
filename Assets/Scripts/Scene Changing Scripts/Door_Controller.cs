using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Controller : MonoBehaviour
{
    public bool isOpen;
    //public Animator animator;
    public GameObject player;
    public loadNextScene loadNextScene;

    public void OpenDoor(){
        isOpen = true;
        Debug.Log("Door Opened");
        //loadNextScene = FindObjectOfType<loadNextScene>();
        if (player != null)
        {
            DontDestroyOnLoad(player);
            SceneManager.LoadScene("Forest");
        }
        else
        {
            Debug.Log("no player to send to the next scene");
        }
        
      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //grabs the player object from trigger collision
        player = collision.gameObject.transform.parent.gameObject;
        
    }
}
