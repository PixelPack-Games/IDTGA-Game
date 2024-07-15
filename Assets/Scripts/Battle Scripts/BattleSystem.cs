using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYER_ONE_TURN, PLAYER_TWO_TURN, PLAYER_THREE_TURN, PLAYER_FOUR_TURN, ENEMY_ONE_TURN, ENEMY_TWO_TURN, ENEMY_THREE_TURN, ENEMY_FOUR_TURN, WON, LOST }
public enum MenuState { START, PLAYER_OPTIONS, PLAYER_ATTACK, ENEMY_ACTION}
public class BattleSystem : NetworkBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject enemyThreePrefab;
    public GameObject enemyFourPrefab;

    //public Transform playerBattleStation;
    //public Transform enemyBattleStation;
    PlayerStats playerOneStats;
    PlayerStats playerTwoStats;
    PlayerStats playerThreeStats;
    PlayerStats playerFourStats;
    EnemyStats enemyOneStats;
    EnemyStats enemyTwoStats;
    EnemyStats enemyThreeStats;
    EnemyStats enemyFourStats;

    GameObject enemyOne;
    GameObject enemyTwo;
    GameObject enemyThree;
    GameObject enemyFour;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public Transform player3SpawnPoint;
    public Transform player4SpawnPoint;

    public Transform enemy1SpawnPoint;
    public Transform enemy2SpawnPoint;
    public Transform enemy3SpawnPoint;
    public Transform enemy4SpawnPoint;
   

    public BattleSetup BattleSetup;

    public BattleState state;

    public GameObject[] enemyPrefabs;
    public GameObject[] enemyGameObjects;
    public Transform[] enemyPositions;
    public GameObject[] playerGameObjects;
    public Transform[] playerPositions;
    public PlayerStats[] PlayerStats;
    public EnemyStats[] allEnemyStats;
    public BattleHUD[] enemyHUDlist;
    public BattleHUD[] playerHUDlist;
    public int enemyCount;


    public Camera BattleCam;
    public Camera OverworldCam;



    //UI
    public GameObject BattleUI;
    public BattleMenus BattleMenus;
    public BattleHUD playerOneHUD;
    public BattleHUD playerTwoHUD;
    public BattleHUD playerThreeHUD;
    public BattleHUD playerFourHUD;
    public BattleHUD enemyOneHUD;
    public BattleHUD enemyTwoHUD;
    public BattleHUD enemyThreeHUD;
    public BattleHUD enemyFourHUD;
    //

    public loadNextScene loadNextScene;
    public BattleSystemNetwork BS_Network;
    public int ClientId;
    int playerIndex = 0;
    public void StartBattle()
    {
        //gets a list of all the players
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        

        
        
        //if (!IsOwner) return;
        BattleUI.SetActive(true);
        enemyCount = Random.Range(1,4);

        //this could be used to randomize the types of enemies in each battle
        enemyPrefabs = new GameObject[] { enemyOnePrefab, enemyTwoPrefab, enemyThreePrefab, enemyFourPrefab };
        //
        enemyGameObjects = new GameObject[] { enemyOne, enemyTwo, enemyThree, enemyFour };
        enemyPositions = new Transform[] { enemy1SpawnPoint, enemy2SpawnPoint, enemy3SpawnPoint, enemy4SpawnPoint };
        allEnemyStats = new EnemyStats[] { enemyOneStats, enemyTwoStats, enemyThreeStats, enemyFourStats };
        enemyHUDlist = new BattleHUD[] {enemyOneHUD, enemyTwoHUD, enemyThreeHUD, enemyFourHUD};
        playerPositions = new Transform[] {player1SpawnPoint, player2SpawnPoint, player3SpawnPoint, player4SpawnPoint};
        playerHUDlist = new BattleHUD[] {playerOneHUD, playerTwoHUD, playerThreeHUD, playerFourHUD};
        PlayerStats = new PlayerStats[] {playerOneStats, playerTwoStats, playerThreeStats, playerFourStats};


        

        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

//Sets up the instance of the player in their respective location with their HUD and stats
   void AddPlayer(){
        for(int i = 0; i < playerGameObjects.Length; i++) 
            Debug.Log("PlayerObjectName: " + playerGameObjects[i].name );
        Debug.Log("playerGameObjects length == " + playerGameObjects.Length );
        Debug.Log("Index: " + ClientId );
        playerGameObjects[ClientId].transform.position = playerPositions[ClientId].position;
        PlayerStats[ClientId] = playerGameObjects[ClientId].GetComponent<PlayerStats>();
        playerHUDlist[ClientId].SetPlayerHUD(PlayerStats[ClientId]);
        playerHUDlist[ClientId].gameObject.SetActive(true);
        playerHUDlist[ClientId].UpdatePlayerHPtext(PlayerStats[ClientId]);

    }

    public void UpdateServer()
    {
        if (IsOwner)
        {
            
            BattleData temp = new BattleData()
            {
                battleState = state,
            };
            //
            if (IsServer || !BS_Network.serverAuth)
            {
                BS_Network.data.Value = temp;
            }
            else
            {
                //Update the server
                BS_Network.transmitDataServerRpc(temp);
                Debug.Log("Data sent to server");
            }
        }
        else
        {
            //grab the correct info from the server
           state = BS_Network.data.Value.battleState;
        }
        
    }

    IEnumerator SetupBattle()
    {
        //if(IsOwner)
        //{
            OverworldCam.gameObject.SetActive(false);
            BattleCam.gameObject.SetActive(true);
        //}
        
        
        AddPlayer();


        //BattleManager.Instance.player.SetActive(false);
        //Debug.Log("player set to inactive");
        
        for (int i = 0; i < enemyCount; i++)
        {
            enemyGameObjects[i] = Instantiate(BattleManager.Instance.enemy, enemyPositions[i].position, Quaternion.identity);
            enemyGameObjects[i].name = "enemy_" + (i+1).ToString();
            allEnemyStats[i] = enemyGameObjects[i].GetComponent<EnemyStats>();
            if (allEnemyStats[i] == null) Debug.Log($"{allEnemyStats[i]} is  null for some reason");
            enemyHUDlist[i].gameObject.SetActive(true);
            enemyHUDlist[i].SetEnemyHUD(allEnemyStats[i]);
            enemyHUDlist[i].UpdateEnemyHPtext(allEnemyStats[i]);
            BattleMenus.buttonList[i].SetActive(true);
        }
        BattleManager.Instance.enemy.SetActive(false);

        
        //UI SETUP
        if (enemyCount == 1)
            BattleMenus.dialogueText.text = "A " + BattleManager.Instance.enemy.name + " approaches!";
        else BattleMenus.dialogueText.text = enemyCount + " " + BattleManager.Instance.enemy.name + "s approaches!";


        //

        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYER_ONE_TURN;
        StartCoroutine(PlayerTurn());
        
    }

    
    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            Debug.Log("Your party has won!");
            BattleMenus.dialogueText.text = "Your party has won!";
            Debug.Log("current scene index: " + SceneManager.GetActiveScene().buildIndex);
            //StartCoroutine(loadNextScene.LoadBattleScene(SceneManager.GetActiveScene().buildIndex-1));
        }
        else if (state == BattleState.LOST)
        {
            Debug.Log("Your party lost...");
            BattleMenus.dialogueText.text = "Your party lost...";
        }
        yield return new WaitForSeconds(2f);
        PlayerStats[playerIndex].transform.position = BattleManager.Instance.playerOnePos;
        BattleCam.gameObject.SetActive(false);
        OverworldCam.gameObject.SetActive(true);
        BattleUI.SetActive(false);
        PlayerStats[playerIndex].GetComponent<PlayerMovement>().inBattle = false;

        for (int i = 0; i < enemyCount; i++)
        {
            Destroy(enemyGameObjects[i]);
            enemyHUDlist[i].gameObject.SetActive(false);
        }
    }

    IEnumerator EnemyOneTurn()
    {
        //TODO: create some enemy ai logic
        //for now it just attacks the player

        //Skips dead enemy's turn
        if (!allEnemyStats[0].enemy.getAlive())
        {
            state = BattleState.ENEMY_TWO_TURN;
            StartCoroutine(EnemyTwoTurn());
        }
        else
        {
            Debug.Log("enemy 1 is attacking");
            BattleMenus.dialogueText.text = allEnemyStats[0].enemy.getName() + "1 attacked " + PlayerStats[playerIndex].player.getName() + "!";
            allEnemyStats[0].enemy.attackAction(PlayerStats[playerIndex].player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + PlayerStats[playerIndex].player.getCurrHealth());
            playerHUDlist[playerIndex].SetHP(PlayerStats[playerIndex].player.getCurrHealth());
            playerOneHUD.UpdatePlayerHPtext(PlayerStats[playerIndex]);

            if (!(PlayerStats[playerIndex].player.getAlive()))
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMY_TWO_TURN;
                StartCoroutine(EnemyTwoTurn());
            }
        }
        
    }

    IEnumerator EnemyTwoTurn()
    {
        //TODO: create some enemy ai logic
        //for now it just attacks the player
        if (allEnemyStats[1] == null)
        {
            state = BattleState.ENEMY_THREE_TURN;
            StartCoroutine(EnemyThreeTurn());
        }
        else if (!allEnemyStats[1].enemy.getAlive() || allEnemyStats[1].enemy == null)
        {
            state = BattleState.ENEMY_THREE_TURN;
            StartCoroutine(EnemyThreeTurn());
        }
        else
        {
            Debug.Log("enemy 2 is attacking");
            BattleMenus.dialogueText.text = allEnemyStats[1].enemy.getName() + "2 attacked " + PlayerStats[playerIndex].player.getName() + "!";
            allEnemyStats[1].enemy.attackAction(PlayerStats[playerIndex].player);
            yield return new WaitForSeconds(2f);

            //update player hp

            Debug.Log("player health =" + PlayerStats[playerIndex].player.getCurrHealth());
            playerOneHUD.SetHP(PlayerStats[playerIndex].player.getCurrHealth());
            playerOneHUD.UpdatePlayerHPtext(PlayerStats[playerIndex]);

            if (!(PlayerStats[playerIndex].player.getAlive()))
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                //change this battle state to enemy 3
                state = BattleState.ENEMY_THREE_TURN;
                StartCoroutine(EnemyThreeTurn());
            }
        }
        
    }

    IEnumerator EnemyThreeTurn()
    {
        //TODO: create some enemy ai logic
        //for now it just attacks the player
        if(allEnemyStats[2] == null)
        {
            state = BattleState.ENEMY_FOUR_TURN;
            StartCoroutine(EnemyFourTurn());
        }
        else if (!allEnemyStats[2].enemy.getAlive())
        {
            state = BattleState.ENEMY_FOUR_TURN;
            StartCoroutine(EnemyFourTurn());
        }else
        {
            Debug.Log("enemy 3 is attacking");
            BattleMenus.dialogueText.text = allEnemyStats[2].enemy.getName() + "3 attacked " + PlayerStats[playerIndex].player.getName() + "!";
            allEnemyStats[2].enemy.attackAction(PlayerStats[playerIndex].player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + PlayerStats[playerIndex].player.getCurrHealth());
            playerHUDlist[playerIndex].SetHP(PlayerStats[playerIndex].player.getCurrHealth());
            playerOneHUD.UpdatePlayerHPtext(PlayerStats[playerIndex]);

            if (!(PlayerStats[playerIndex].player.getAlive()))
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.ENEMY_FOUR_TURN;
                StartCoroutine(EnemyFourTurn());
            }
        }
    }
    IEnumerator EnemyFourTurn()
    {
        //TODO: create some enemy ai logic
        //for now it just attacks the player
        if (allEnemyStats[3] == null)
        {
            state = BattleState.PLAYER_ONE_TURN;
            StartCoroutine(PlayerTurn());
        }
        else if (!allEnemyStats[3].enemy.getAlive())
        {
            state = BattleState.PLAYER_ONE_TURN;
            StartCoroutine(PlayerTurn());
        }
        else
        {
            Debug.Log("enemy 4 is attaacking");
            BattleMenus.dialogueText.text = allEnemyStats[3].enemy.getName() + "4 attacked " + playerOneStats.player.getName() + "!";
            allEnemyStats[3].enemy.attackAction(playerOneStats.player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + playerOneStats.player.getCurrHealth());
            playerOneHUD.SetHP(playerOneStats.player.getCurrHealth());
            playerOneHUD.UpdatePlayerHPtext(PlayerStats[playerIndex]);

            if (!(playerOneStats.player.getAlive()))
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYER_ONE_TURN;
                StartCoroutine(PlayerTurn());
            }
        } 
    }

    IEnumerator PlayerTurn()
    {
        //SOMETHING HAS TO HAPPEN HERE TO FIND WHICH CHARACTER IS WHICH
        updatePlayerButtons();
        yield return new WaitForSeconds(1f);
        Debug.Log("playerONEturn");
        BattleMenus.dialogueText.text = "Choose an action: ";
        BattleMenus.MainHUD.SetActive(false);
        BattleMenus.ChoiceHUD.SetActive(true);
        //onAttackButton();  
    }
    
    //UI BUTTONS//
    public void onAttackButton()
    {
        if (state != BattleState.PLAYER_ONE_TURN)
            return;
        BattleMenus.ToggleMenu(BattleMenus.ChoiceHUD);
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
    }

    public void AttackEnemyOneButton()
    {
        if (state != BattleState.PLAYER_ONE_TURN || !allEnemyStats[0].enemy.getAlive())
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[0], 0));
    }

    public void AttackEnemyTwoButton()
    {
        if (state != BattleState.PLAYER_ONE_TURN || !allEnemyStats[1].enemy.getAlive())
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[1], 1));
    }

    public void AttackEnemyThreeButton()
    {
        if (state != BattleState.PLAYER_ONE_TURN || !allEnemyStats[2].enemy.getAlive())
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[2], 2));
    }

    public void AttackEnemyFourButton()
    {
        if (state != BattleState.PLAYER_ONE_TURN || !allEnemyStats[3].enemy.getAlive())
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[3], 3));
    }
    ////////////////

    void updatePlayerButtons()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (!allEnemyStats[i].enemy.getAlive()) BattleMenus.buttonList[i].SetActive(false);
        }
    }
    IEnumerator PlayerAttack(PlayerStats AttackingPlayer, EnemyStats DefendingEnemy, int enemyIndex)
    {
        Debug.Log("Player Attacks!");
        BattleMenus.dialogueText.text = AttackingPlayer.Name + " attacks " + DefendingEnemy.Name;
        //deal damage
        AttackingPlayer.player.attackAction(DefendingEnemy.enemy);
        yield return new WaitForSeconds(2f);
        //check if dead
        string enemyName = DefendingEnemy.gameObject.name;
        Debug.Log($"{enemyName} health = " + DefendingEnemy.enemy.getCurrHealth());

        enemyHUDlist[enemyIndex].SetHP(DefendingEnemy.enemy.getCurrHealth());
        enemyHUDlist[enemyIndex].UpdateEnemyHPtext(allEnemyStats[enemyIndex]);
        
        if (AllEnemiesAreDead())
        {
            Debug.Log("All enemies are dead");
            //End battle

            //for now its just win state
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            //enemy turn
            //IF THERE ARE MORE PLAYERS, MAKE THIS PLAYER TWO TURN
            // also encapuls
            //for now its just true PLEASE CHANGE
            if(true)
            {
                state = BattleState.ENEMY_ONE_TURN;
                StartCoroutine(EnemyOneTurn());
                
            }
            
        }
        
        //give exp
    }

    //TODO: create functions for checking if all players are dead and if all enemies are dead to shorten code

    bool AllEnemiesAreDead()
    {
        //implement for loop
        int count = 0;
        for(int i = 0; i< enemyCount; i++)
        {
            if (!allEnemyStats[i].enemy.getAlive()) count++;
        }
        if (count == enemyCount) return true;
        return false;
    }

    private void Update()
    {
        if (state == BattleState.PLAYER_ONE_TURN)
        {
            if (Input.GetKeyDown(KeyCode.E) && allEnemyStats[0].enemy.getAlive())
            {
                StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[0], 0));
            }else if(Input.GetKeyDown(KeyCode.R) && allEnemyStats[1].enemy.getAlive() && allEnemyStats[1].enemy != null)
            {
                StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[1], 1));
            }
            else if (Input.GetKeyDown(KeyCode.T) && allEnemyStats[2].enemy.getAlive() && allEnemyStats[2].enemy != null)
            {
                StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[2], 2));
            }
            else if (Input.GetKeyDown(KeyCode.Y) && allEnemyStats[3].enemy.getAlive() && allEnemyStats[3].enemy != null)
            {
                StartCoroutine(PlayerAttack(PlayerStats[playerIndex], allEnemyStats[3], 3));
            }
        }
    }
}

