using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField]
        private ItemGroupObject itemGroup;
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private Transform itemContainer;
        
        Dictionary<int, Dictionary<int, GameObject>> objects;

        private void Awake()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        private void Start()
        {
            objects = new Dictionary<int, Dictionary<int, GameObject>>();
            PhotonNetwork.RaiseEvent((int)Events.GET_WORLD, "", RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private void OnEvent(EventData obj)
        {
            Events e = (Events)obj.Code;

            if (e == Events.GET_WORLD)
                GetWorldHandler((object[])obj.CustomData);
        }
        
        private void GetWorldHandler(object[] data)
        {
            int count = (int)data[0];

            for (int i = 0; i < count; ++i)
            {
                int index = i * 3 + 1;
                int x = (Int32)data[index];
                int y = (Int32)data[index + 1];
                int item = (Int32)data[index + 2];
                
                CreateItem(x, y, item);
            }
        }

        public bool HasItem(Vector2 position)
        {
            int x = (int)position.x;
            int y = (int)position.y;

            // SELECT item_id FROM Main.World WHERE x=x AND y=y;
            String sqlCmd = String.Format("SELECT item_id FROM Main.World WHERE x={0} AND y={1};", x, y);
            Debug.Log(sqlCmd);

            //if (!items.ContainsKey(x) || !items[x].ContainsKey(y))
            //    return false;

            return false;// items[x][y] >= 0;
        }

        public bool ItemAt(Vector2 position, out int item)
        {
            item = -1;

            int x = (int)position.x;
            int y = (int)position.y;

            // SELECT item_id FROM Main.World WHERE x=x AND y=y;
            String sqlCmd = String.Format("SELECT item_id FROM Main.World WHERE x={0} AND y={1};", x, y);
            Debug.Log(sqlCmd);

            //if (!items.ContainsKey(x) || !items[x].ContainsKey(y))
            //    return false;

            //item = items[x][y];

            return false;// item >= 0;
        }

        private void CreateItem(int x, int y, int item)
        {
            Vector2 position = new Vector2((float)x, (float)y);

            GameObject go = Instantiate(itemPrefab);
            go.transform.position = position;
            go.transform.SetParent(itemContainer);
            go.GetComponent<SpriteRenderer>().color = itemGroup.colors[item];

            if (!objects.ContainsKey(x))
                objects.Add(x, new Dictionary<int, GameObject>());

            objects[x][y] = go;
        }

        public bool PlaceItem(Vector2 position, int item)
        {
            bool result = HasItem(position);

            if (!result)
            {
                int x = (int)position.x;
                int y = (int)position.y;

                //GameObject go = Instantiate(itemPrefab);
                //go.transform.position = position;
                //go.transform.SetParent(itemContainer);
                //go.GetComponent<SpriteRenderer>().color = itemGroup.colors[item];

                //if (!objects.ContainsKey(x))
                //    objects.Add(x, new Dictionary<int, GameObject>());

                //objects[x][y] = go;

                // TODO - Insert item into world db
                // INSERT INTO Main.World(item_id, x, y) VALUES (go, x, y);
                String sqlCmd = String.Format("INSERT INTO Main.World(item_id, x, y) VALUES ({0}, {1}, {2});", item, x, y);
                Debug.Log(sqlCmd);
            }

            return !result;
        }

        public void PickItem(string username, int index, Vector2 position)
        {
            object[] data = new object[] {
                username,
                index,
                (int)position.x,
                (int)position.y
            };

            PhotonNetwork.RaiseEvent((int)Events.PICKUP_ITEM, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }
}