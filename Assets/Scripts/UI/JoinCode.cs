using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class JoinCode : MonoBehaviour
{
    public GameObject joinCode;
    private TextMeshProUGUI textComponent;
    private string code;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void storeJoinCode(){
        textComponent = joinCode.GetComponent<TextMeshProUGUI>();
        code = textComponent.text;
        Debug.Log(code);
    }
    public string getJoinCode(){
        return code;
    }
}
