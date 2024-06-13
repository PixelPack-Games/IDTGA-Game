using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Controller : MonoBehaviour
{
    public bool isOpen;

    public void OpenChest(){
        isOpen = true;
        Debug.Log("Chest Opened");
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
