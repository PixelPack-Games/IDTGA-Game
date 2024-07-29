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

public enum BattleState { 
    START,
    PLAYER_ONE_TURN,
    PLAYER_TWO_TURN,
    PLAYER_THREE_TURN,
    PLAYER_FOUR_TURN,
    ENEMY_ONE_TURN,
    ENEMY_TWO_TURN,
    ENEMY_THREE_TURN,
    ENEMY_FOUR_TURN,
    WON,
    LOST }
public enum MenuState { START, PLAYER_OPTIONS, PLAYER_ATTACK, ENEMY_ACTION}
public class BattleSystem : NetworkBehaviour
{
    GameObject playerOne;
    GameObject playerTwo;
    GameObject playerThree;
    GameObject playerFour;

    GameObject enemyOnePrefab;
    GameObject enemyTwoPrefab;
    GameObject enemyThreePrefab;
    GameObject enemyFourPrefab;

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

    Transform player1OverworldPos;
    public Transform player2OverworldPos;
    Transform player3OverworldPos;
    Transform player4OverworldPos;

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

    GameObject[] enemyPrefabs;
    GameObject[] enemyGameObjects;
    Transform[] enemyPositions;
    GameObject[] playerGameObjects;
    Transform[] playerPositions;
    PlayerStats[] PlayerStats;
    EnemyStats[] allEnemyStats;
    BattleHUD[] enemyHUDlist;
    BattleHUD[] playerHUDlist;

    Vector3[] playerOverworldPositions;
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
    public Network network;
    public int ClientId;
    public int currentPlayerIndex;
    public int currentEnemyIndex;
    int attackedPlayerIndex;
    int playerCount;
    public bool isCoroutineRunning = false;
    public void StartBattle(ref Collider2D enemyCollider)
    {
        ClientId = (int)NetworkManager.Singleton.LocalClientId;
        //gets a list of all the players
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        playerCount = playerGameObjects.Length;
        playerOverworldPositions = new Vector3[playerGameObjects.Length];
        for(int i = 0; i< playerGameObjects.Length; i++)
        {
            playerOverworldPositions[i] = playerGameObjects[i].transform.position;
        }
        currentPlayerIndex = -1;
        currentEnemyIndex = -1;

        
        
        //if (!IsOwner) return;
        BattleUI.SetActive(true);
        //enemyCount = Random.Range(1,4);
        enemyCount = 2;
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
        
        Debug.Log("THE ACTUAL CLIENT ID OF THE FIRST CLIENT IS: " + NetworkManager.Singleton.LocalClientId);
        

        state = BattleState.START;

        //network.updateBattleState(ref playerOneStats.player, ref enemyOneStats.enemy, state);
        StartCoroutine(SetupBattle(enemyCollider));
    }

//Sets up the instance of the player in their respective location with their HUD and stats
   void AddPlayers(Collider2D enemyCollider){
        for(int i = 0; i < playerCount; i++) 
        {
            playerGameObjects[i].name = "Player_" + (i+1).ToString();
            playerGameObjects[i].GetComponent<PlayerMovement>().inBattle = true;
            playerGameObjects[i].GetComponent<PlayerInput>().inBattle = true;
            //NEEDS THE SERVER TO UPDATE THE PLAYER POSITIONS
            playerGameObjects[i].transform.position = playerPositions[i].position;
            playerGameObjects[i].GetComponent<Network>().StartPositions(playerPositions[i].position, ref enemyCollider);
            playerGameObjects[i].GetComponent<Network>().networkInBattle = true;
            PlayerStats[i] = playerGameObjects[i].GetComponent<PlayerStats>();
            playerHUDlist[i].SetPlayerHUD(PlayerStats[i]);
            playerHUDlist[i].gameObject.SetActive(true);
            playerHUDlist[i].UpdatePlayerHPtext(PlayerStats[i]);
            Debug.Log("Placed: " + playerGameObjects[i].name );

        }
            

        Debug.Log("playerGameObjects length == " + playerCount );
        Debug.Log("Index: " + ClientId );

    }


    IEnumerator SetupBattle(Collider2D enemyCollider)
    {
        isCoroutineRunning = true;
        //if(IsOwner)
        //{
            OverworldCam.gameObject.SetActive(false);
            BattleCam.gameObject.SetActive(true);
        //}
        
        
        AddPlayers(enemyCollider);


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

        for (int bogus = 0; bogus < playerCount; bogus++)
        {
            playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref PlayerStats[bogus].player, ref allEnemyStats[0].enemy, ref state);
        }


        //UI SETUP
        if (enemyCount == 1)
            BattleMenus.dialogueText.text = "A " + BattleManager.Instance.enemy.name + " approaches!";
        else BattleMenus.dialogueText.text = enemyCount + " " + BattleManager.Instance.enemy.name + "s approaches!";


        //

        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYER_ONE_TURN;

        for (int bogus = 0; bogus < playerCount; bogus++)
        {
            playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref PlayerStats[bogus].player, ref allEnemyStats[0].enemy, ref state);
        }
        isCoroutineRunning = false;
        //StartCoroutine(PlayerTurn());
        
    }

    
    IEnumerator EndBattle()
    {
        isCoroutineRunning = true;
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
        for(int i = 0; i < playerCount; i++)
        {
            PlayerStats[i].transform.position = playerOverworldPositions[i];
            PlayerStats[i].GetComponent<PlayerMovement>().inBattle = false;
        }
        
        BattleCam.gameObject.SetActive(false);
        OverworldCam.gameObject.SetActive(true);
        BattleUI.SetActive(false);
        for(int i = 0; i < playerCount; i++)
            PlayerStats[i].GetComponent<PlayerInput>().inBattle = false;

        for (int i = 0; i < enemyCount; i++)
        {
            Destroy(enemyGameObjects[i]);
            enemyHUDlist[i].gameObject.SetActive(false);
        }
    }

    IEnumerator EnemyTurn()
    {
        isCoroutineRunning = true;
        //TODO: create some enemy ai logic
        //for now it just attacks the player

        //Skips dead enemy's turn
        currentEnemyIndex++;
        if(currentEnemyIndex == 4) currentEnemyIndex = 0;
        /*switch(currentEnemyIndex) 
            {
            case 0:
                state = BattleState.ENEMY_ONE_TURN;
                break;
            case 1:
                state = BattleState.ENEMY_TWO_TURN;
                break;
            case 2:
                state = BattleState.ENEMY_THREE_TURN;
                break;
            case 3:
                state = BattleState.ENEMY_FOUR_TURN;
                break;
            }*/
        Debug.Log("We in enemy turny");
        if(allEnemyStats[currentEnemyIndex] == null)
        {
            if(currentEnemyIndex == 3)
            {
                state = BattleState.PLAYER_ONE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerTurn());
            }
                
            else
            {
                state += 1;
                isCoroutineRunning = false;
                //StartCoroutine(EnemyTurn());
            } 
        }
        else if (!allEnemyStats[currentEnemyIndex].enemy.getAlive())
        {
            Debug.Log("This person is dead: enemy " + currentEnemyIndex);
            if(currentEnemyIndex == 3)
            {
                Debug.Log("player turn now ig on: enemy " + currentEnemyIndex);
                state = BattleState.PLAYER_ONE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerTurn());
                
            }  
            else
            {
                state += 1;
                isCoroutineRunning = false;
                //StartCoroutine(EnemyTurn());
                Debug.Log("emeney " + currentEnemyIndex);
            }
                
        }
        else
        {
            Debug.Log("enemy " + currentEnemyIndex + " is attacking");
            attackedPlayerIndex = Random.Range(0,playerCount-1);
            int enemyNumber = currentEnemyIndex +1;
            BattleMenus.dialogueText.text = allEnemyStats[currentEnemyIndex].enemy.getName() + " " + enemyNumber + " attacked " + PlayerStats[attackedPlayerIndex].player.getName() + "!";
            allEnemyStats[currentEnemyIndex].enemy.attackAction(PlayerStats[attackedPlayerIndex].player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + PlayerStats[attackedPlayerIndex].player.getCurrHealth());
            playerHUDlist[attackedPlayerIndex].SetHP(PlayerStats[attackedPlayerIndex].player.getCurrHealth());
            playerOneHUD.UpdatePlayerHPtext(PlayerStats[attackedPlayerIndex]);

            if (AllPlayersAreDead())
            {
                state = BattleState.LOST;
                isCoroutineRunning = false;
                //StartCoroutine(EndBattle());
            }
            else
            {
                switch(currentEnemyIndex) 
                {
                case 0:
                    state = BattleState.ENEMY_TWO_TURN;
                    isCoroutineRunning = false;
                    //StartCoroutine(EnemyTurn());
                    break;
                case 1:
                    state = BattleState.ENEMY_THREE_TURN;
                    isCoroutineRunning = false;
                    //StartCoroutine(EnemyTurn());
                    break;
                case 2:
                    state = BattleState.ENEMY_FOUR_TURN;
                    isCoroutineRunning = false;
                    //StartCoroutine(EnemyTurn());
                    break;
                case 3:
                    state = BattleState.PLAYER_ONE_TURN;
                    isCoroutineRunning = false;
                    //StartCoroutine(PlayerTurn());
                    break;
                }
            }
        }

        
    }


    //UI BUTTONS//
    public void onAttackButton()
    {
        if (currentPlayerIndex != ClientId)
            return;
        BattleMenus.ToggleMenu(BattleMenus.ChoiceHUD);
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
    }

    public void AttackEnemyOneButton()
    {
        if (currentPlayerIndex != ClientId)
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[currentPlayerIndex], allEnemyStats[0], 0));
    }

    public void AttackEnemyTwoButton()
    {
        if (currentPlayerIndex != ClientId)
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[currentPlayerIndex], allEnemyStats[1], 1));
    }

    public void AttackEnemyThreeButton()
    {
        if (currentPlayerIndex != ClientId)
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[currentPlayerIndex], allEnemyStats[2], 2));
    }

    public void AttackEnemyFourButton()
    {
        if (currentPlayerIndex != ClientId)
            return;
        BattleMenus.ToggleMenu(BattleMenus.AttackHUD);
        BattleMenus.ToggleMenu(BattleMenus.MainHUD);
        StartCoroutine(PlayerAttack(PlayerStats[currentPlayerIndex], allEnemyStats[3], 3));
    }
    ////////////////

    void updatePlayerButtons()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (!allEnemyStats[i].enemy.getAlive()) BattleMenus.buttonList[i].SetActive(false);
        }
    }

        IEnumerator PlayerTurn()
    {
        isCoroutineRunning = true;
        //SOMETHING HAS TO HAPPEN HERE TO FIND WHICH CHARACTER IS WHICH
        Debug.Log(state);
        currentPlayerIndex++;
        if(currentPlayerIndex == 4) 
            currentPlayerIndex = 0;
        /*switch(currentPlayerIndex) 
            {
            case 0:
                state = BattleState.PLAYER_ONE_TURN;
                break;
            case 1:
                state = BattleState.PLAYER_TWO_TURN;
                break;
            case 2:
                state = BattleState.PLAYER_THREE_TURN;
                break;
            case 3:
                state = BattleState.PLAYER_FOUR_TURN;
                break;
            }*/

        for (int bogus = 0; bogus < playerCount; bogus++)
        {
            //playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref PlayerStats[currentPlayerIndex].player, ref allEnemyStats[0].enemy, ref state);
        }

        if (PlayerStats[currentPlayerIndex] == null)
        {
            if(currentPlayerIndex == 3)
            {
                state = BattleState.ENEMY_ONE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(EnemyTurn());
            }

            else
            {
                state += 1;
                Debug.Log("next player turn.. state is: " + state);
                isCoroutineRunning = false;
                yield return new WaitForSeconds(1f);
                //StartCoroutine(PlayerOneTurn());
            }

        }   
        else if(!PlayerStats[currentPlayerIndex].player.getAlive())
        {
            if(currentPlayerIndex == 3)
            {
                state = BattleState.ENEMY_ONE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(EnemyTurn());
            }
                
            else
            {
                state += 1;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerOneTurn());
            }
        }   
        else
        {
            updatePlayerButtons();
            yield return new WaitForSeconds(1f);
            Debug.Log("player " + currentPlayerIndex +  " turn");
            BattleMenus.dialogueText.text = "Choose an action: ";
            BattleMenus.MainHUD.SetActive(false);
            BattleMenus.ChoiceHUD.SetActive(true);
        //onAttackButton();  
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

        for (int bogus = 0; bogus < playerCount; bogus++)
        {
            playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref AttackingPlayer.player, ref DefendingEnemy.enemy, ref state);
        }

        if (AllEnemiesAreDead())
        {
            Debug.Log("All enemies are dead");
            //End battle

            //for now its just win state
            state = BattleState.WON;

            for (int bogus = 0; bogus < playerCount; bogus++)
            {
                playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref AttackingPlayer.player, ref DefendingEnemy.enemy, ref state);
            }

            isCoroutineRunning = false;
            //StartCoroutine(EndBattle());
        }
        else
        {
            switch(currentPlayerIndex) 
            {
            case 0:
            
                state = BattleState.PLAYER_TWO_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerOneTurn());
                break;
            case 1:
                state = BattleState.PLAYER_THREE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerOneTurn());
                break;
            case 2:
                state = BattleState.PLAYER_FOUR_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(PlayerOneTurn());
                break;
            case 3:
                state = BattleState.ENEMY_ONE_TURN;
                isCoroutineRunning = false;
                //StartCoroutine(EnemyTurn());
                break;
            }

            for (int bogus = 0; bogus < playerCount; bogus++)
            {
                playerGameObjects[bogus].GetComponent<Network>().updateBattleState(ref AttackingPlayer.player, ref DefendingEnemy.enemy, ref state);
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

 bool AllPlayersAreDead()
    {
        //implement for loop
        int count = 0;
        for(int i = 0; i< playerGameObjects.Length ; i++)
        {
            if (!PlayerStats[i].player.getAlive()) count++;
        }
        if (count == playerCount) return true;
        return false;
    }
    private void Update()
    {
        if (!isCoroutineRunning)
        {
            Debug.Log("running: " + state  );
            switch (state)
            {
                case BattleState.START:
                    break;
                case BattleState.PLAYER_ONE_TURN:
                case BattleState.PLAYER_TWO_TURN:
                case BattleState.PLAYER_THREE_TURN:
                case BattleState.PLAYER_FOUR_TURN:
                    
                    StartCoroutine(PlayerTurn());
                    //isCoroutineRunning = true;
                    break;
                case BattleState.ENEMY_ONE_TURN:
                case BattleState.ENEMY_TWO_TURN:
                case BattleState.ENEMY_THREE_TURN:
                case BattleState.ENEMY_FOUR_TURN:
                    StartCoroutine(EnemyTurn());
                    //isCoroutineRunning = true;
                    break;
                case BattleState.WON:
                case BattleState.LOST:
                    StartCoroutine(EndBattle());
                    //isCoroutineRunning = true;
                    break;
            }
        }
    }
}

