using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static jyj_playerBehavior;

public class jyj_playerData : NetworkBehaviour
{
    //private NetworkVariable<PlayerData> data;
    //[SerializeField] private bool serverAuth;
    [SerializeField] private jyj_playerBehavior player;
    private NetworkTimer timer;
    private const float SERVER_TICK_RATE = 30f;
    private const int BUFFER_SIZE = 1024;

    //Client only
    private CircularBuffer<PlayerData> clientDataBuffer;
    private CircularBuffer<PlayerInputData> clientInputBuffer;
    private PlayerData lastServerUpdate;
    private PlayerData lastProccessedData;

    //Server only
    CircularBuffer<PlayerData> serverDataBuffer;
    Queue<PlayerInputData> serverInputQueue;

    [SerializeField] private float reconciliationThreshold = 50f;

    private struct PlayerData : INetworkSerializable
    {
        private float x, y;
        public int tick;

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
            serializer.SerializeValue(ref tick);
        }
    }

    private struct PlayerInputData : INetworkSerializable
    {
        public int tick;
        public Vector3 input;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref input);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            return;
        }

        //Debug.Log("Server authority status: " + serverAuth);
    }

    private void Awake()
    {
        //NetworkVariableWritePermission perm = serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        //data = new NetworkVariable<PlayerData>(writePerm: perm);
        timer = new NetworkTimer(SERVER_TICK_RATE);
        clientDataBuffer = new CircularBuffer<PlayerData>(BUFFER_SIZE);
        clientInputBuffer = new CircularBuffer<PlayerInputData>(BUFFER_SIZE);
        serverDataBuffer = new CircularBuffer<PlayerData>(BUFFER_SIZE);
        serverInputQueue = new Queue<PlayerInputData>();
    }

    // Update is called once per frame
    void Update()
    {
        timer.update(Time.deltaTime);

        /* if (IsOwner)
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
        }*/
    }

    private void FixedUpdate()
    {
        while(timer.shouldTick())
        {
            handleClientTick();
            handleServerTick();
        }
    }

    private PlayerData processMove(PlayerInputData input)
    {
        player.movePlayer(input.input);

        return new PlayerData()
        {
            tick = input.tick,
            pos = transform.position,
        };
    }

    private PlayerData simulateMove(PlayerInputData input)
    {
        Physics.simulationMode = SimulationMode.Script;
        processMove(input);
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.simulationMode = SimulationMode.FixedUpdate;

        return new PlayerData()
        {
            tick = input.tick,
            pos = transform.position
        };
    }

    private void handleClientTick()
    {
        if (!IsClient || !IsOwner)
        {
            return;
        }

        int currTick = timer.currTick;
        int bufferIndex = currTick % BUFFER_SIZE;
        PlayerInputData input = new PlayerInputData()
        {
            tick = currTick,
            input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)
        };

        clientInputBuffer.add(input, bufferIndex);
        //processMove(input);

        transmitInputDataServerRpc(input);

        PlayerData playerData = processMove(input);
        clientDataBuffer.add(playerData, bufferIndex);
        handleServerReconciliation();
    }

    private void handleServerTick()
    {
        if (!IsServer)
        {
            return;
        }

        int bufferIndex = -1;

        while (serverInputQueue.Count > 0)
        {
            PlayerInputData input = serverInputQueue.Dequeue();
            bufferIndex = input.tick % BUFFER_SIZE;
            //PlayerData playerData = simulateMove(input);
            PlayerData playerData = processMove(input);
            serverDataBuffer.add(playerData, bufferIndex);
        }

        if (bufferIndex == -1)
        {
            return;
        }

        transmitDataClientRpc(serverDataBuffer.get(bufferIndex));
    }

    private void handleServerReconciliation()
    {
        if (!shouldReconcile())
        {
            return;
        }

        float err;
        int bufferIndex;
        PlayerData data = default;

        bufferIndex = lastServerUpdate.tick % BUFFER_SIZE;

        if (bufferIndex - 1 < 0)
        {
            return;
        }

        data = IsHost ? serverDataBuffer.get(bufferIndex - 1) : clientDataBuffer.get(bufferIndex - 1);
        err = Vector3.Distance(data.pos, clientDataBuffer.get(bufferIndex).pos);

        if (err > reconciliationThreshold)
        {
            reconcile(data);
        }

        lastProccessedData = lastServerUpdate;
    }

    private bool shouldReconcile()
    {
        bool isNewData = !lastServerUpdate.Equals(default);
        bool isUndefinedOrDifferent = lastProccessedData.Equals(default) || !lastProccessedData.Equals(lastServerUpdate);

        return (isNewData && isUndefinedOrDifferent);
    }

    private void reconcile(PlayerData data)
    {
        transform.position = data.pos;

        if (!data.Equals(lastServerUpdate))
        {
            return;
        }

        clientDataBuffer.add(data, data.tick);

        int tick = lastServerUpdate.tick;

        while(tick < timer.currTick)
        {
            int bufferIndex = tick % BUFFER_SIZE;
            PlayerData newData = processMove(clientInputBuffer.get(bufferIndex));
            clientDataBuffer.add(newData, bufferIndex);
            tick++;
        }
    }

    /*[ServerRpc]
    private void transmitDataServerRpc(PlayerData temp)
    {
        data.Value = temp;
    }*/

    [ServerRpc]
    private void transmitInputDataServerRpc(PlayerInputData input)
    {
        serverInputQueue.Enqueue(input);
    }

    [ClientRpc]
    private void transmitDataClientRpc(PlayerData playerData)
    {
        if (!IsOwner)
        {
            return;
        }

        lastServerUpdate = playerData;
    }
}
