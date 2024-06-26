using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door_Controller : MonoBehaviour
{
    public bool isOpen;
    //public Animator animator;
    

    public void OpenDoor(){
        isOpen = true;
        Debug.Log("Door Opened");
        SceneManager.LoadScene("Jerry Test Scene");
      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
