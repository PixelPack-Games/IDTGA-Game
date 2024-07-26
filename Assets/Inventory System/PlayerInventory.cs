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

        for (int bogus = 0; bogus < objects.Length; bogus++)
        {
            if (!objects[bogus].name.Equals(inventoryObjectName))
            {
                continue;
            }

            InventoryScreen = objects[bogus];
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventory Opened");
            InventoryScreen.SetActive(!InventoryScreen.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[28];
    }
}
