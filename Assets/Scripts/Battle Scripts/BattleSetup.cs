using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public Transform player3SpawnPoint;
    public Transform player4SpawnPoint;
    public Transform enemy1SpawnPoint;
    public Transform enemy2SpawnPoint;
    public Transform enemy3SpawnPoint;
    public Transform enemy4SpawnPoint;
    public GameObject player;

    void Awake()
    {
        Transform[] EnemyPositions = new Transform[] { enemy1SpawnPoint, enemy2SpawnPoint, enemy3SpawnPoint, enemy4SpawnPoint };

        player = Instantiate(BattleManager.Instance.player, player1SpawnPoint.position, Quaternion.identity);
        //TODO: edit this to accomodate multiple players and stop more enemies from spawning when multiple players join
        //TODO: Edit the randomness to reflect how many players are in the current party
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject enemy = Instantiate(BattleManager.Instance.enemy, EnemyPositions[i].position, Quaternion.identity); ;
        }
        

        //disable player and enemy from prev scene to avoid duplicates
        BattleManager.Instance.player.SetActive(false);
        Debug.Log("player set to inactive");
        BattleManager.Instance.enemy.SetActive(false);


    }
}