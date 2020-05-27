using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Inventory : MonoBehaviour
{

    [SerializeField]
    private InventoryManager manager;
    [SerializeField]
    private GridManager grid;

    private PlayerMovement movement;

    public int size { get; private set; }
    public int index { get; private set; }
    public int[] items { get; private set; }

    private string username;

    private void Awake()
    {
        size = 4;
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        index = 0;
        username = AccountManager.Instance().username;

        items = new int[size];
        for (int i = 0; i < size; ++i)
            items[i] = -1;

        items[0] = 0;
        items[1] = 1;

        String sqlCmd = String.Format(
            "SELECT item0, item1, item2, item3 from Main.Inventory WHERE username='{0}';", 
            username
        );
        Debug.Log(sqlCmd);

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

        if (Input.GetKeyDown(KeyCode.E))
           Pickup();

        if (Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    private void Pickup() 
    {
        if (items[index] >= 0)
            return;

        Vector3 target = transform.position + movement.direction;

        if (!grid.PickItem(target, out items[index]))
            return;


        String sqlCmd = String.Format(
            "UPDATE Main.Inventory SET item{0}={1} WHERE username='{2}';", 
            index, items[index], username
        );
        Debug.Log(sqlCmd);

        manager.ReloadData();
    }

    private void Drop()
    {
        if (items[index] < 0)
            return;

        Vector3 target = transform.position + movement.direction;

        if (!grid.PlaceItem(target, items[index]))
            return;

        items[index] = -1;

        // TODO - Update player inventory in db
        String sqlCmd = String.Format(
            "UPDATE Main.Inventory SET item{0}={1} WHERE username='{2}';", 
            index, -1, username
        );
        Debug.Log(sqlCmd);

        manager.ReloadData();
    }

}
