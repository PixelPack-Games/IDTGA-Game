using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public enum BattleState { START, PLAYERTURN, ENEMY_ONE_TURN, ENEMY_TWO_TURN, ENEMY_THREE_TURN, ENEMY_FOUR_TURN, WON, LOST }
public class BattleSystem : MonoBehaviour
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
    public EnemyStats[] allEnemyStats;
    public int enemyCount;
    

    void Start()
    {
        enemyCount = Random.Range(1,4);

        //this could be used to randomize the types of enemies in each battle
        enemyPrefabs = new GameObject[] { enemyOnePrefab, enemyTwoPrefab, enemyThreePrefab, enemyFourPrefab };

        enemyGameObjects = new GameObject[] { enemyOne, enemyTwo, enemyThree, enemyFour };
        enemyPositions = new Transform[] { enemy1SpawnPoint, enemy2SpawnPoint, enemy3SpawnPoint, enemy4SpawnPoint };
        allEnemyStats = new EnemyStats[] { enemyOneStats, enemyTwoStats, enemyThreeStats, enemyFourStats };
       
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {


       
        playerOne = Instantiate(BattleManager.Instance.player, player1SpawnPoint.position, Quaternion.identity);
        BattleManager.Instance.player.SetActive(false);
        Debug.Log("player set to inactive");
        
        for (int i = 0; i < enemyCount; i++)
        {
            enemyGameObjects[i] = Instantiate(BattleManager.Instance.enemy, enemyPositions[i].position, Quaternion.identity);
            enemyGameObjects[i].name = "enemy_" + (i+1).ToString();
            allEnemyStats[i] = enemyGameObjects[i].GetComponent<EnemyStats>();
            if (allEnemyStats[i] == null) Debug.Log($"{allEnemyStats[i]} is  null for some reason");

        }
        BattleManager.Instance.enemy.SetActive(false);





        yield return new WaitForSeconds(2f);

        playerOneStats = playerOne.GetComponent<PlayerStats>();

        if (playerOneStats.player != null)
        {
            //Debug.Log(playerOneStats.player.getCurrHealth());
        }
        else
        {
            Debug.LogError("player1.player is null");
        }
        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
        
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            Debug.Log("player has won");
        }else if (state == BattleState.LOST)
        {
            Debug.Log("player has lost");
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
            allEnemyStats[0].enemy.attackAction(playerOneStats.player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + playerOneStats.player.getCurrHealth());

            if (!(playerOneStats.player.getAlive()))
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
            allEnemyStats[1].enemy.attackAction(playerOneStats.player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + playerOneStats.player.getCurrHealth());

            if (!(playerOneStats.player.getAlive()))
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
            allEnemyStats[2].enemy.attackAction(playerOneStats.player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + playerOneStats.player.getCurrHealth());

            if (!(playerOneStats.player.getAlive()))
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
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
        else if (!allEnemyStats[3].enemy.getAlive())
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
        else
        {
            Debug.Log("enemy 4 is attaacking");
            allEnemyStats[3].enemy.attackAction(playerOneStats.player);
            yield return new WaitForSeconds(2f);

            //update player hp
            Debug.Log("player health =" + playerOneStats.player.getCurrHealth());

            if (!(playerOneStats.player.getAlive()))
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
            }
        } 
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("playerturn");
        yield return new WaitForSeconds(1f);
        
        
        onAttackButton();  
    }
    
    //this is for when the UI gets implemented
    public void onAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        //StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack(PlayerStats AttackingPlayer, EnemyStats DefendingEnemy)
    {
        Debug.Log("Player Attacks!");
        //deal damage
        AttackingPlayer.player.attackAction(DefendingEnemy.enemy);
        yield return new WaitForSeconds(2f);
        //check if dead
        string enemyName = DefendingEnemy.gameObject.name;
        Debug.Log($"{enemyName} health = " + DefendingEnemy.enemy.getCurrHealth());
        
        if (AllEnemiesAreDead())
        {
            Debug.Log("All enemies are dead");
            //End battle

            //for now its just win state
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            //enemy turn
            //IF THERE ARE MORE PLAYERS, MAKE THIS PLAYER TWO TURN
            // also check if the next players are dead so you can skip their turn
            if(playerTwo == null && playerThree == null && playerFour == null)
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
        if (state == BattleState.PLAYERTURN)
        {
            if (Input.GetKeyDown(KeyCode.E) && allEnemyStats[0].enemy.getAlive())
            {
                StartCoroutine(PlayerAttack(playerOneStats, allEnemyStats[0]));
            }else if(Input.GetKeyDown(KeyCode.R) && allEnemyStats[1].enemy.getAlive() && allEnemyStats[1].enemy != null)
            {
                StartCoroutine(PlayerAttack(playerOneStats, allEnemyStats[1]));
            }
            else if (Input.GetKeyDown(KeyCode.T) && allEnemyStats[2].enemy.getAlive() && allEnemyStats[2].enemy != null)
            {
                StartCoroutine(PlayerAttack(playerOneStats, allEnemyStats[2]));
            }
            else if (Input.GetKeyDown(KeyCode.Y) && allEnemyStats[3].enemy.getAlive() && allEnemyStats[3].enemy != null)
            {
                StartCoroutine(PlayerAttack(playerOneStats, allEnemyStats[3]));
            }
        }
    }
}
