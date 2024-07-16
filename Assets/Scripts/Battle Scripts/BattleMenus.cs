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
    public GameObject attackButtonOne;
    public GameObject attackButtonTwo;
    public GameObject attackButtonThree;
    public GameObject attackButtonFour;
    public GameObject[] buttonList;

    private void Awake()
    {
        buttonList = new GameObject[] { attackButtonOne, attackButtonTwo, attackButtonThree, attackButtonFour};
        
    }
    
    public void ToggleMenu(GameObject Menu)
    {
        if(Menu.gameObject.activeInHierarchy) Menu.gameObject.SetActive(false);
        else Menu.gameObject.SetActive(true);
    }

}