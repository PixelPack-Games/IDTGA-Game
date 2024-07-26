using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using System.Text;
using System;

public class DisplayDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public float textSpeed;
    public int charLimit;
    private TextMeshProUGUI textComponent;
    private string[] lines;
    private int index;
    void Start(){
        
    }

    void Update(){
        if(gameObject.activeSelf == true){
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)){
                if(textComponent.text == lines[index]){
                    NextLine();
                }
                else{
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    public void Instantiate(){
        if(!gameObject.activeSelf){
            gameObject.SetActive(true);
        }
        var obj = Instantiate(dialogueBox,Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        textComponent = obj.GetComponent<TextMeshProUGUI>();
        index = 0;
    }

    public void SetDialogue(string[] dialogue){
        lines = dialogue;
    }

    public void StartDialogue(){
        Debug.Log("Starting Dialogue");
        StartCoroutine(TypeLine());
    }

    public void EndDialogue(){
        gameObject.SetActive(false);
        Destroy(gameObject.transform.GetChild(0).gameObject);
    }

    IEnumerator TypeLine(){
        foreach (char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{
            EndDialogue();
        }
    }
}
