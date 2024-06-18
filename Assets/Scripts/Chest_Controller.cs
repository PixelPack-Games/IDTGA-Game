using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Controller : MonoBehaviour
{
    public bool isOpen;
    //public Animator animator;
    

    public void OpenChest(){
        isOpen = true;
        Debug.Log("Chest Opened");
        //animator.SetBool("IsOpen", isOpen);
    }
    public void TransformToMimic(){
        
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
