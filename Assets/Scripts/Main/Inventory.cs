using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Inventory : MonoBehaviour
{

    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform itemContainer;
    [SerializeField]
    private InventoryManager manager;

    private PlayerMovement movement;

    public int size { get; private set; }
    public int index { get; private set; }
    public Item[] items { get; private set; }

    private void Awake()
    {
        size = 4;
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        index = 0;

        items = new Item[size];
        for (int i = 0; i < size; ++i)
            items[i] = null;

        items[0] = new Item();
        items[0].color = new Color(1, 0, 0, 1);
        manager.ReloadData();
    }
     
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
            manager.UpdateIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
            manager.UpdateIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            manager.UpdateIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
            manager.UpdateIndicator();
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //    Pickup();

        if (Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    public bool Set(int index, Item item)
    {
        if (items[index] != null)
            return false;

        items[index] = item;
        return true;
    }

    private void Drop()
    {
        if (items[index] != null)
            return;

        GameObject go = Instantiate(itemPrefab);
        go.transform.position = transform.position + movement.direction;
        go.transform.SetParent(itemContainer);
        go.GetComponent<SpriteRenderer>().color = items[index].color;

        items[index] = null;

        manager.ReloadData();
    }

}
