using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public GameObject player;
    public GameObject enemy;
    public Vector3 playerOnePos;
    public BattleTrigger battleTrigger;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //check if we have exited scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //SUB PLAYER FOR PARTY
    public void SetBattleData(GameObject player, GameObject enemy, Vector3 overworldPos)
    {
        this.player = player;
        this.enemy = enemy;
        this.playerOnePos = overworldPos;
    }

    public GameObject getBattlePlayer()
    {
        return this.player;
    }
    //get rid of sceneLoaded when destroying this obj

}
