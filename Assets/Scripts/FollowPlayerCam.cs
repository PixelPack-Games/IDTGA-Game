using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Unity.Netcode;

public class FollowPlayerCam : NetworkBehaviour
{
    public CinemachineVirtualCamera vcam;
    public GameObject player;
    public Transform FollowTarget;
    
    public bool isAttached = false;
    BattleSystem battleSystem;
    GameObject[] players = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        battleSystem = FindAnyObjectByType<BattleSystem>();
    }

    void Update()
    {
        
            if (!isAttached)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
                /*for(int i = players.Length-1; i >= 0 ; i++)
                {
                    GameObject playerObj = players[i].transform.root.gameObject;
                    Debug.Log("Player[" + i + "] name: " + playerObj.name);
                    if(players[i].GetComponent<Network>()!= null)
                    {
                        Debug.Log("Player[" + i + "] OwnerClientId: " + playerObj.GetComponent<Network>().OwnerClientId);
                        if(players[i].GetComponent<Network>().OwnerClientId == (ulong)i)
                        {
                            
                            player  = players[i];
                            isAttached = true;
                            break;
                            
                        }
                        //Debug.Log( "OwnerClientID : "+ OwnerClientId);
                    }
                
                }*/
                if(players.Length != 0)
                {
                    GameObject playerObj = players[players.Length-1].transform.root.gameObject;
                    Debug.Log("Player[" + (players.Length-1) + "] name: " + playerObj.name);
                    player  = players[players.Length-1];
                    battleSystem.ClientId = players.Length-1;
                    isAttached = true;
                }
                
            }
            if (isAttached && player != null)
            {
                FollowTarget = player.transform;
                vcam.LookAt = FollowTarget;
                vcam.Follow = FollowTarget;
                isAttached = true;
            }
        
        
    }
}
