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
    private GameObject obj;
    private TextMeshProUGUI textComponent;
    private string[] lines;
    private int index;
    void Start(){
        
    }

    void Update(){
        if(gameObject.activeSelf == true){
            if(Input.GetMouseButtonDown(0)){
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
        gameObject.SetActive(true);
        obj = Instantiate(dialogueBox, new Vector3(960,190,0), Quaternion.identity, transform);
        textComponent = obj.GetComponent<TextMeshProUGUI>();
        index = 0;
    }

    public void SetDialogue(string dialogue){
        int arraySize = (dialogue.Length + (charLimit-1)) / charLimit;
        lines = new string[arraySize];

        string[] words = dialogue.Split(' ');
        StringBuilder sb = new StringBuilder();
        int i = 0;
        for(int word = 0; word < words.Length; word++){
            sb.Append(words[word]);
            sb.Append(' ');
            if (sb.Length >= charLimit){
                lines[i] = sb.ToString();
                sb.Clear();
                i++;
            }
        }
        if(sb.Length > 0){
            lines[i] = sb.ToString();
        }
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
