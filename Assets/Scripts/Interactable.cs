using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool inRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange){
            if(Input.GetKeyDown(interactKey)){
                interactAction.Invoke();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player")){
            inRange = true;
            Debug.Log("In range: Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player")){
            inRange = false;
            Debug.Log("No longer in range");
        }
    }
}
