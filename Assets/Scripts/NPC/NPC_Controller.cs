using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public GameObject DialogueScreen;
    [TextArea(3, 5)]
    public string[] dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact(){
        if(DialogueScreen.activeSelf == false){
            Debug.Log("Interacted with NPC");
            DialogueScreen.GetComponent<DisplayDialogue>().Instantiate();
            DialogueScreen.GetComponent<DisplayDialogue>().SetDialogue(dialogue);
            DialogueScreen.GetComponent<DisplayDialogue>().StartDialogue();
        }
    }
}
