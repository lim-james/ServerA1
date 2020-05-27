using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField]
    private ItemGroupObject itemGroup;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform itemContainer;

    Dictionary<int, Dictionary<int, int>> items;
    Dictionary<int, Dictionary<int, GameObject>> objects;

    private void Start() 
    {
        items = new Dictionary<int, Dictionary<int, int>>();
        objects = new Dictionary<int, Dictionary<int, GameObject>>();

        // SELECT x, y, item_id from Main.World;
        string sqlCmd = "SELECT x, y, item_id from Main.World;";
        Debug.Log(sqlCmd);
    }

    public bool HasItem(Vector2 position) 
    {
        int x = (int)position.x;
        int y = (int)position.y;

        // SELECT item_id FROM Main.World WHERE x=x AND y=y;
        String sqlCmd = String.Format("SELECT item_id FROM Main.World WHERE x={0} AND y={1};", x, y);
        Debug.Log(sqlCmd);

        if (!items.ContainsKey(x) || !items[x].ContainsKey(y)) 
            return false; 

        return items[x][y] >= 0;
    }

    public bool ItemAt(Vector2 position, out int item) 
    {
        item = -1;

        int x = (int)position.x;
        int y = (int)position.y;

        // SELECT item_id FROM Main.World WHERE x=x AND y=y;
        String sqlCmd = String.Format("SELECT item_id FROM Main.World WHERE x={0} AND y={1};", x, y);
        Debug.Log(sqlCmd);

        if (!items.ContainsKey(x) || !items[x].ContainsKey(y)) 
            return false; 

        item = items[x][y];
        return item >= 0;
    }

    public bool PlaceItem(Vector2 position, int item) 
    {
        bool result = HasItem(position);

        if (!result) 
        {
            int x = (int)position.x;
            int y = (int)position.y;
            if (!items.ContainsKey(x))
                items.Add(x, new Dictionary<int, int>());

            items[x][y] = item;

            GameObject go = Instantiate(itemPrefab);
            go.transform.position = position;
            go.transform.SetParent(itemContainer);
            go.GetComponent<SpriteRenderer>().color = itemGroup.colors[item];

            if (!objects.ContainsKey(x))
                objects.Add(x, new Dictionary<int, GameObject>());

            objects[x][y] = go;

            // TODO - Insert item into world db
            // INSERT INTO Main.World(item_id, x, y) VALUES (go, x, y);
            String sqlCmd = String.Format("INSERT INTO Main.World(item_id, x, y) VALUES ({0}, {1}, {2});", item, x, y);
            Debug.Log(sqlCmd);
        }

        return !result;
    }

    public bool PickItem(Vector2 position, out int item) 
    {
        bool result = ItemAt(position, out item);

        if (result) 
        {
            Debug.Log("Picking");
            int x = (int)position.x;
            int y = (int)position.y;
            items[x][y] = -1;

            Destroy(objects[x][y]);

            // TODO - Remove item from world db
            // DELETE FROM Main.World WHERE x=1 AND y=1;
            String sqlCmd = String.Format("DELETE FROM Main.World WHERE x={0} AND y={1};", x, y);
            Debug.Log(sqlCmd);
        }

        return result;
    }
}
