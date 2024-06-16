using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using static jyj_playerBehavior;
using static Countdown;
using static jyj_betterNetworkTransform;

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
    [SerializeField] private float extrapolationLimit = 0.5f;
    [SerializeField] private float extrapolationMultiplier = 1.2f;

    //Server only
    CircularBuffer<PlayerData> serverDataBuffer;
    Queue<PlayerInputData> serverInputQueue;

    [SerializeField] private float reconciliationThreshold = 50f;
    private Countdown cooldown;
    [SerializeField] private float cooldownValue;
    private jyj_betterNetworkTransform bnt;
    private PlayerData extrapolationData;
    private Countdown extrapolationCooldown;

    private struct PlayerData : INetworkSerializable
    {
        public Vector3 pos;
        public int tick;
        public ulong objectId;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref pos);
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref objectId);
        }
    }

    private struct PlayerInputData : INetworkSerializable
    {
        public int tick;
        public Vector3 input;
        public DateTime timeStamp;
        public ulong objectId;
        public Vector3 pos;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref input);
            serializer.SerializeValue(ref timeStamp);
            serializer.SerializeValue(ref objectId);
            serializer.SerializeValue(ref pos);
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
        cooldown = new Countdown(cooldownValue);

        cooldown.onTimerStart += () =>
        {
            extrapolationCooldown.stop();
        };

        bnt = GetComponent<jyj_betterNetworkTransform>();
        extrapolationCooldown = new Countdown(extrapolationLimit);

        extrapolationCooldown.onTimerStart += () =>
        {
            cooldown.stop();
            bnt.authMode = AuthMode.SERVER;
            bnt.SyncPositionX = false;
            bnt.SyncPositionY = false;
        };

        extrapolationCooldown.onTimerStop += () =>
        {
            bnt.authMode = AuthMode.CLIENT;
            bnt.SyncPositionX = true;
            bnt.SyncPositionY = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        timer.update(Time.deltaTime);
        cooldown.tick(Time.deltaTime);
        extrapolationCooldown.tick(Time.deltaTime);
        extrapolate();

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
            objectId = input.objectId
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
            input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0),
            timeStamp = DateTime.Now,
            objectId = NetworkObjectId,
            pos = transform.position
        };

        PlayerData playerData = processMove(input);
        clientInputBuffer.add(input, bufferIndex);
        transmitInputDataServerRpc(input);
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
        PlayerInputData input = default;

        while (serverInputQueue.Count > 0)
        {
            input = serverInputQueue.Dequeue();
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
        handleExtrapolation(serverDataBuffer.get(bufferIndex), calcLatency(input));
    }

    private void handleExtrapolation(PlayerData data, float latency)
    {
        if (latency < extrapolationLimit && latency > Time.fixedDeltaTime)
        {
            if (extrapolationData.pos != default)
            {
                data = extrapolationData;
            }

            Vector3 adjust = data.pos * (1 + latency * extrapolationMultiplier);
            extrapolationData.pos = adjust;
        }
        else
        {
            extrapolationCooldown.stop();
        }
    }

    private void extrapolate()
    {
        if (!IsServer && extrapolationCooldown.isRunning)
        {
            transform.position += extrapolationData.pos;
        }
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
            cooldown.start();
        }

        lastProccessedData = lastServerUpdate;
    }

    private bool shouldReconcile()
    {
        bool isNewData = !lastServerUpdate.Equals(default);
        bool isUndefinedOrDifferent = lastProccessedData.Equals(default) || !lastProccessedData.Equals(lastServerUpdate);

        return (isNewData && isUndefinedOrDifferent && !cooldown.isRunning && !extrapolationCooldown.isRunning);
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

    static float calcLatency(PlayerInputData input)
    {
        return (DateTime.Now - input.timeStamp).Milliseconds / 1000f;
    }
}
