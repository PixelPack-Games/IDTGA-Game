using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("Server started.");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server stopped.");
    }

    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("Client started.");
    }

    public override void OnStopClient()
    {
        Debug.Log("Client stopped.");
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Client connected: " + conn.address);

        // Limit number of connections
        if (NetworkServer.connections.Count > 4)
        {
            conn.Disconnect();
            Debug.Log("Connection refused: Maximum number of clients reached.");
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Client disconnected: " + conn.address);
    }
}
