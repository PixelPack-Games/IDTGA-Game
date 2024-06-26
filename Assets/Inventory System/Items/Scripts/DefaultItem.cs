using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    void Awake()
    {
        type = ItemType.Default;
    }
}
