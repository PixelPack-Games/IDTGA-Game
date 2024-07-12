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
    void Update()
    {
        
    }
}

public struct BattleData : INetworkSerializable
{
    LinkedList<Player> playerList;
    //public Enemy[] enemyList;
    LinkedList<Enemy> enemyList;
    public BattleState BattleState;
    public MenuState MenuState;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerList);
        serializer.SerializeValue(ref enemyList);
        serializer.SerializeValue(ref BattleState);
        serializer.SerializeValue(ref MenuState);

    }
}

