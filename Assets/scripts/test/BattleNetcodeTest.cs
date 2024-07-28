using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BattleNetcodeTest : MonoBehaviour
{
    [SerializeField] private Network network;
    [SerializeField] private GameObject player;
    private Enemy enemy;
    private bool playerInit = false;
    private Player playerStats;

    private void Awake()
    {
        enemy = new Enemy("test", "test1", gameObject, 10, 5, 5, 5, 0);
    }

    private void Update()
    {
        if (!playerInit && player != null)
        {
            playerStats = player.GetComponent<PlayerStats>().player;
            playerInit = true;
        }

        if (network == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !network.networkInBattle)
        {
            network.networkInBattle = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            network.updateBattleState(ref playerStats, ref enemy, ++network.state);
        }

        Debug.Log("Current State: " + network.state);
    }
}
