using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static jyj_playerBehavior;

public class jyj_playerData : NetworkBehaviour
{
    private NetworkVariable<PlayerData> data;
    [SerializeField] private bool serverAuth;
    [SerializeField] private jyj_playerBehavior player;

    private struct PlayerData : INetworkSerializable
    {
        private float x, y;

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
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        Debug.Log("Server authority status: " + serverAuth);
    }

    private void Awake()
    {
        NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        data = new NetworkVariable<PlayerData>(writePerm: perm);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            return;
        }

        player.turnOff = true;
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
            }
        }
        else
        {
            Vector3 lagDist = data.Value.pos - transform.position;

            if (lagDist.magnitude > 5f)
            {
                transform.position = data.Value.pos;
                lagDist = Vector3.zero;
            }

            if (lagDist.magnitude < 0.11f)
            {
                player.movePlayer(Vector3.zero);
            }
            else
            {
                player.movePlayer(lagDist.normalized);
            }
        }
    }

    [ServerRpc]
    private void transmitDataServerRpc(PlayerData temp)
    {
        data.Value = temp;
    }
}
