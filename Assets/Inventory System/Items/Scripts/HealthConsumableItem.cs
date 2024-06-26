using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Consumable Item", menuName = "Inventory System/Items/Consumable/Health")]
public class HealthConsumableItem : ItemObject
{
    public float restoreHealthValue;
    public void Awake(){
        type = ItemType.HealthConsumable;
    }
}
