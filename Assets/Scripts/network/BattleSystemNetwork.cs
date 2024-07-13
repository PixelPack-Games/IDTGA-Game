using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BattleSystemNetwork : NetworkBehaviour
{
    public NetworkVariable<BattleData> data;
    [SerializeField] public bool serverAuth;
    void Start()
    {

    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        data = new NetworkVariable<BattleData>(writePerm: perm);
    }

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
        data.Value = temp;
    }
}



public struct BattleData : INetworkSerializable
{
    LinkedList<Player> playerList;
    //public Enemy[] enemyList;
    LinkedList<Enemy> enemyList;
    public BattleState battleState;
    public MenuState MenuState;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerList);
        serializer.SerializeValue(ref enemyList);
        serializer.SerializeValue(ref battleState);
        serializer.SerializeValue(ref MenuState);

    }
}

