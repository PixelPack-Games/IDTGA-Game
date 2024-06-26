using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Inventory System/Items/Equipment/Armor")]
public class ArmorItem : ItemObject
{
    public float defenseValue;
    public void Awake(){
        type = ItemType.Armor;
    }
}
