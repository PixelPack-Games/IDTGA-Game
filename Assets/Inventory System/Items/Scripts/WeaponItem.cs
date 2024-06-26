using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Inventory System/Items/Equipment/Weapon")]
public class WeaponItem : ItemObject
{
    public float damageValue;
    public void Awake(){
        type = ItemType.Weapon;
    }
}
