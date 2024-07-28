using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Network : NetworkBehaviour
{
    public NetworkVariable<PlayerData> data;
    [SerializeField] public bool serverAuth;
    public bool inBattle = false;
    public BattleState state = 0;
    [SerializeField] Sprite[] sprites;
    [SerializeField] RuntimeAnimatorController[] renderers;
    private static int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Animator animator = GetComponent<Animator>();

        if (!IsOwner)
        {
            renderer.sprite = sprites[playerCount];
            animator.runtimeAnimatorController = renderers[playerCount];
            playerCount++;
            return;
        }

        renderer.sprite = sprites[NetworkManager.Singleton.LocalClientId];
        animator.runtimeAnimatorController = renderers[NetworkManager.Singleton.LocalClientId];
        playerCount++;
        //Debug.Log("Server authority status: " + serverAuth);
    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        data = new NetworkVariable<PlayerData>(writePerm: perm);
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattle)
        {
            return;
        }

        if (IsOwner)
        {
            PlayerData temp = new PlayerData()
            {
                pos = transform.position,
            };

            if (IsServer || !serverAuth)
            {
                data.Value = temp;
            }
            else
            {
                transmitDataServerRpc(temp);
                //data.Value = temp;
            }
        }
        else
        {
            transform.position = data.Value.pos;
        }
    }

    public void updateBattleState(ref Player player, ref Enemy enemy, BattleState state)
    {
        if (!inBattle)
        {
            return;
        }

        if (IsOwner)
        {
            PlayerData temp = new PlayerData()
            {
                pos = transform.position,
                state = state,
                playerHealth = player.getCurrHealth(),
                enemyHeath = enemy.getCurrHealth()
            };

            if (IsServer || !serverAuth)
            {
                data.Value = temp;
            }
            else
            {
                transmitDataServerRpc(temp);
            }
        }
        else
        {
            transform.position = data.Value.pos;
            //TODO: make an actual state transfer in BattleSystem.cs
            state = data.Value.state;
            player.setCurrhealth(data.Value.playerHealth);
            enemy.setCurrhealth(data.Value.enemyHeath);
        }
    }

    [ServerRpc]
    public void transmitDataServerRpc(PlayerData temp)
    {
        transmitDataClientRpc(temp);
    }

    [ServerRpc]
    public void destroyActorServerRpc(Entity entity)
    {
        entity.die(GetComponent<NetworkObject>());
    }

    [ClientRpc]
    private void transmitDataClientRpc(PlayerData temp)
    {
        if (IsOwner)
        {
            return;
        }
         
        //TODO: add interpolation for smoother connectivity
        data.Value = temp;
    }
}

public struct PlayerData : INetworkSerializable
{
    private float x, y;
    public BattleState state;
    public int playerHealth, enemyHeath; //used when battle state is in a player attack or an enemy attack

    internal Vector3 pos
    {
        get => new Vector3(x, y, 0);
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref y);
        serializer.SerializeValue(ref state);
        serializer.SerializeValue(ref playerHealth);
    }
}
