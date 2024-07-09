using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Network : NetworkBehaviour
{
    public NetworkVariable<PlayerData> data;
    [SerializeField] public bool serverAuth;
    [SerializeField] private PlayerMovement moveScript;
    private loadNextScene sceneScript;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        Debug.Log("Server authority status: " + serverAuth);
        sceneScript = FindObjectOfType<loadNextScene>();
    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        data = new NetworkVariable<PlayerData>(writePerm: perm);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            PlayerData temp = new PlayerData()
            {
                pos = transform.position
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
            if (moveScript.inBattle)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.position = data.Value.pos;
        }
    }

    public void loadNextSceneAndPopulate(GameObject player, GameObject enemy)
    {
        BattleManager.Instance.SetBattleData(player, enemy);
        //makes sure these objects can be referenced in other scenes
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(enemy);
        //make sure the player is inBattle now
        moveScript.inBattle = true;
        //this gets the next scene from the build settings
        sceneScript.LoadNextLevel();
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

    [ServerRpc]
    public void loadNextSceneServerRpc(PlayerData temp)
    {
        //loadNextSceneAndPopulate(temp.self, temp.enemy);
        loadNextSceneClientRpc(temp);
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

    [ClientRpc]
    private void loadNextSceneClientRpc(PlayerData temp)
    {
        if (IsOwner)
        {
            return;
        }

        loadNextSceneAndPopulate(temp.self, temp.enemy);
    }
}

public struct PlayerData : INetworkSerializable
{
    private float x, y;
    public int sceneIndex;
    public GameObject self;
    public GameObject enemy;

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
        serializer.SerializeValue(ref sceneIndex);
    }
}
