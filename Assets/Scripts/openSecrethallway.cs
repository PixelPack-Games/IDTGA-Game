using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openSecrethallway : MonoBehaviour
{
public GameObject trainingDummy1;
    public GameObject trainingDummy2;
    public GameObject trainingDummy3;
    public GameObject hallway;
    bool allDummiesDefeated = false;
    void Update()
    {
        if(!allDummiesDefeated)
        {
            if(!trainingDummy1.activeInHierarchy && !trainingDummy2.activeInHierarchy && !trainingDummy3.activeInHierarchy)
            {
                allDummiesDefeated = true;
                Debug.Log("all defeated");
                hallway.SetActive(false);
            }
        } 
    }
}
