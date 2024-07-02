using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerStarted()
    {
        base.OnServerStarted();
        Debug.Log("Server started.");
    }

    public override void OnServerStopped()
    {
        base.OnServerStopped();
        Debug.Log("Server stopped.");
    }

    public override void OnClientConnected(ulong clientId)
    {
        base.OnClientConnected(clientId);
        Debug.Log($"Client connected: {clientId}");

        // Limit number of connections
        if (NetworkManager.Singleton.ConnectedClients.Count > 4)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            Debug.Log("Connection refused: Maximum number of clients reached.");
        }
    }

    public override void OnClientDisconnect(ulong clientId)
    {
        base.OnClientDisconnect(clientId);
        Debug.Log($"Client disconnected: {clientId}");
    }
}