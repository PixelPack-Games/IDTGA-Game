using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 * @brief Entity class that all game objects should derive from
 */

public class Entity : INetworkSerializable
{
    //Properties
    private string id; //a unique name for the entity abject, use this over name in code to check for equality
    private string name; //usename as the entity's display name
    private NetworkObject networkObject;
    private GameObject gameObject;

    //Contructors
    public Entity()
    {
        
    }

    public Entity(string name, string id, GameObject gameObject)
    {
        this.name = name;
        this.id = id;
        this.gameObject = gameObject;
        networkObject = gameObject.GetComponent<NetworkObject>();
    }

    //Implementations
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref name);
    }

    //Functions
    public void die(NetworkObject net)
    {
        if (!networkObject)
        {
            Debug.Log("No network object assigned");
            networkObject = net;
        }

        /*if (!networkObject.IsOwner)
        {
            return;
        }*/

        networkObject.Despawn(true);
    }

    //Getters
    public string getName()
    {
        return (name);
    }

    private string getID()
    {
        return (id);
    }

    private NetworkObject getNetworkObject()
    {
        return (networkObject);
    }

    private GameObject getGameObject()
    {
        return (gameObject);
    }

    //Setters
    private void setName(string name)
    {
        this.name = name;
    }

    private void setID(string id)
    {
        this.id = id;
    }

    private void setNetworkObject(NetworkObject networkObject)
    {
        this.networkObject = networkObject;
    }

    private void setGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}