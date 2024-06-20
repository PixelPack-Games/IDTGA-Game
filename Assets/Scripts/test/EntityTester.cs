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
        entity = new Actor("Test", "test", this.gameObject, 5, 1, 1, 1);
        Actor actor = (Actor)entity;
        Debug.Log(actor.getName() + " created with " + actor.getCurrHealth() + " health");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject)
        {
            //Actor actor = (Actor)entity;

            if (!IsOwner)
            {
                return;
            }

            if (IsClient)
            {
               
                destroyActorServerRpc(entity);
                return;
            }

            //Actor actor = (Actor)entity;
            entity.die(GetComponent<NetworkObject>());
        }
    }
}
