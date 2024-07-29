using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Network : NetworkBehaviour
{
    public NetworkVariable<PlayerData> data;
    [SerializeField] public bool serverAuth;
    public  bool networkInBattle = false;
    
    [SerializeField] private BattleState battleState = 0;
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
        playerCount++;;
        Debug.Log("Server authority status: " + serverAuth);
    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        data = new NetworkVariable<PlayerData>(writePerm: perm);
    }

    // Update is called once per frame
    void Update()
    {
        if (networkInBattle)
        {
            //transform.position = data.Value.pos;
            //networkInBattle = data.Value.inBattle;
            //state = data.Value.state;
            gameObject.GetComponent<PlayerInput>().inBattle = true;
            
            if (data.Value.enemyCollider == null)
            {
                return;
            }

            gameObject.GetComponent<BattleTrigger>().triggerBattle(data.Value.enemyCollider);

            PlayerData temp = new PlayerData()
            {
                pos = data.Value.pos,
                inBattle = data.Value.inBattle,
                state = data.Value.state,
                enemyCollider = null
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

            return;
        }

        if (IsOwner)
        {

            if (data.Value.inBattle)
            {
                Debug.Log("In a battle");
                return;
            }

            PlayerData temp;

            temp = new PlayerData()
                {
                    pos = transform.position,
                    inBattle = data.Value.inBattle,
                    state = data.Value.state,
                    enemyCollider = data.Value.enemyCollider
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
            //networkInBattle = data.Value.inBattle;
            //state = data.Value.state;
        }
    }

    public void updateBattleState(ref Player player, ref Enemy enemy, ref BattleState state)
    {
        if (!networkInBattle)
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

            //battleState = state;
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

    public void StartPositions(Vector3 playerPos, ref Collider2D enemyCollider)
    {
        if (IsOwner)
        {
            PlayerData temp = new PlayerData()
            {
                inBattle = true,
                pos = playerPos,
                state = BattleState.START,
                enemyCollider = enemyCollider
            };

            if (IsServer || !serverAuth)
            {
                Debug.Log("running server update");
                data.Value = temp;
            }
            else
            {
                Debug.Log("running Client update");
                transmitDataServerRpc(temp);
            }
        }
        /*else
        {
            networkInBattle = true;
            transform.position = playerPos;
            state = BattleState.START;
            Debug.Log("")
        }*/
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
        networkInBattle = data.Value.inBattle;
    }
}

public struct PlayerData : INetworkSerializable
{
    private float x, y;
    public BattleState state;
    public bool inBattle;
    public int playerHealth, enemyHeath; //used when battle state is in a player attack or an enemy attack
    public Collider2D enemyCollider;

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
        serializer.SerializeValue(ref inBattle);
        serializer.SerializeValue(ref state);
        serializer.SerializeValue(ref playerHealth);
        //serializer.SerializeValue(ref enemyCollider);
    }
}
