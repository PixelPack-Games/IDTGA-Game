using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GetJoinCode : MonoBehaviour
{
    private string code;
    private GameObject joinCodeStore;
    // Start is called before the first frame update
    void Start()
    {
        int clientId = (int)NetworkManager.Singleton.LocalClientId;
        if(clientId != 0){
            var obj = this.GameObject();
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void getCode(){
        GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name.Equals("JoinCodeStore"))
                joinCodeStore = objects[i];
        }

        JoinCode joinCode = joinCodeStore.GetComponent<JoinCode>();
        code = joinCode.getJoinCode();
        Debug.Log(code);
        var obj = this.GameObject();
        TextMeshProUGUI textComponent = obj.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = code;
    }
}
