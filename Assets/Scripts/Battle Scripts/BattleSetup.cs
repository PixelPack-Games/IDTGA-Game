using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    void Start()
    {
        GameObject player = Instantiate(BattleManager.Instance.player, playerSpawnPoint.position, Quaternion.identity);
        GameObject enemy = Instantiate(BattleManager.Instance.enemy, enemySpawnPoint.position, Quaternion.identity);

        //disable player and enemy from prev scene to avoid duplicates
        BattleManager.Instance.player.SetActive(false);
        BattleManager.Instance.enemy.SetActive(false);


    }
}