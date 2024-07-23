using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BattleSystemNetwork : NetworkBehaviour
{
    public NetworkVariable<BattleData> battleData;
    [SerializeField] public bool serverAuth;
    [SerializeField] BattleNetcodeTest tester; //clean up later
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        battleData = new NetworkVariable<BattleData>(writePerm: perm);
    }

    public void updateBattle(BattleState newState, ref LinkedList<Player> playerStates, ref LinkedList<Enemy> enemyStates)
    {
        if (IsOwner)
        {
            BattleData temp = new BattleData()
            {
                //players = playerStates,
                //enemeies = enemyStates,
                state = newState
            };

            if (IsServer || !serverAuth)
            {
                battleData.Value = temp;
                //tester.syncLocalStats(battleData.Value);
            }
            else
            {
                transmitBattleDataServerRpc(temp);
            }
        }
        else
        {
            //TODO: write function in battle system to sync the entity states and battle state
            //tester.syncLocalStats(battleData.Value);
        }
    }

    [ServerRpc]
    private void transmitBattleDataServerRpc(BattleData temp)
    {
        transmitBattleDataClientRpc(temp);
    }

    [ClientRpc]
    private void transmitBattleDataClientRpc(BattleData temp)
    {
        Debug.Log("Sending to clients");

        if (IsOwner)
        {
            return;
        }

        battleData.Value = temp;
    }

    /*
    [ServerRpc]
    public void destroyActorServerRpc(Entity entity)
    {
        entity.die(GetComponent<NetworkObject>());
    }

    [ServerRpc]
    public void transmitDataServerRpc(BattleData temp)
    {
        transmitDataClientRpc(temp);
    }

    [ClientRpc]
    private void transmitDataClientRpc(BattleData temp)
    {
        if (IsOwner)
        {
            return;
        }
        battleData.Value = temp;
    }
    */
}



public struct BattleData : INetworkSerializable
{
    //public LinkedList<Player> players;
    //public LinkedList<Enemy> enemeies;
    public BattleState state;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        //serializer.SerializeValue(ref players);
        //serializer.SerializeValue(ref enemeies);
        serializer.SerializeValue(ref state);
    }
}
