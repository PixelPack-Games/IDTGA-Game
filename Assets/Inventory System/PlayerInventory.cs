using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject InventoryScreen;
    [SerializeField] private string inventoryObjectName;
    // Start is called before the first frame update

    private void Awake()
    {
        findInventoryScreen();
    }

    private void findInventoryScreen()
    {
        GameObject[] objects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].name.Equals(inventoryObjectName))
            {
                continue;
            }

            InventoryScreen = objects[i];
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            Debug.Log(_item.Id);
            inventory.AddItem(_item, 1);
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        if (InventoryScreen == null)
        {
            findInventoryScreen();
            return;
        }
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[28];
    }
}
