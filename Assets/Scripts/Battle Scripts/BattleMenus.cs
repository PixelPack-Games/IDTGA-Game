using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMenus : MonoBehaviour
{

    public TextMeshProUGUI dialogueText;
   
    public GameObject MainHUD;
    public GameObject ChoiceHUD;
    public GameObject AttackHUD;


    // Start is called before the first frame update
    public void ToggleMenu(GameObject Menu)
    {
        if(Menu.gameObject.activeInHierarchy) Menu.gameObject.SetActive(false);
        else Menu.gameObject.SetActive(true);
    }

}
