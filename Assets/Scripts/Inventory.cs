using System.Collections;
using System.Collections.Generic;
using ParrelSync.NonCore;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryObject inventory;
    
    public void OnTriggerEnter2D(Collider2D other){
        var item = other.GetComponent<Item>();
        if(item){
            inventory.AddItem(item.item, 1);
            Destroy(other.GameObject());
        }
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            inventory.Save();
        }
        if(Input.GetKeyDown(KeyCode.L)){
            inventory.Load();
        }
    }

    private void OnApplicationQuit(){
        inventory.Container.Clear();
    }
}
