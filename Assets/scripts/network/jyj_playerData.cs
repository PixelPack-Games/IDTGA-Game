using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class jyj_playerData : NetworkBehaviour
{
    private NetworkVariable<PlayerData> data;
    [SerializeField] private bool serverAuth;

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
            transform.position = data.Value.pos;
        }
    }

    [ServerRpc]
    private void transmitDataServerRpc(PlayerData temp)
    {
        transmitDataClientRpc(temp);
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
