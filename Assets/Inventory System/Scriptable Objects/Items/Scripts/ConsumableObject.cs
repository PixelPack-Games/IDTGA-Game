using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public bool HealthItem;
    public float RestoreHealthValue;
    public bool ManaItem;
    public float RestoreManaValue;
    public void Awake()
    {
        type = ItemType.Consumable;
    }

    public void UseItem(){
        if(HealthItem){
            
            //restore health
        }
        if(ManaItem){

            //restore mana
        }
    }
}
