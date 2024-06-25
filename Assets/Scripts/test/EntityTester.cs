using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EntityTester : Network
{
    private Entity entity;
    // Start is called before the first frame update
    void Start()
    {
        entity = new Rogue("Test", "test", this.gameObject, 5, 1, 1, 1, 10);
        Rogue player = (Rogue)entity;
        Debug.Log(player.getName() + " created with " + player.getCurrHealth() + " health");
        LinkedList<Entity> weapons = player.getWeapons();
        weapons.iterate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject)
        {
            if (!IsOwner)
            {
                return;
            }

            /*if (IsClient)
            {
               
                destroyActorServerRpc(entity);
                return;
            }

            entity.die(GetComponent<NetworkObject>());*/

            Rogue player = (Rogue)entity;
            Debug.Log("Removing: " + player.getWeapons().remove("dagger"));
            player.getWeapons().iterate();
        }
    }
}
