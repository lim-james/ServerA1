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
            else if (e == Events.REMOVE_ITEM)
                RemoveHandler((object[])obj.CustomData);
            else if (e == Events.CREATE_ITEM)
                CreateHandler((object[])obj.CustomData);
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

        private void RemoveHandler(object[] data)
        {
            int x = (int)data[0];
            int y = (int)data[1];

            RemoveItem(x, y);
        }

        private void CreateHandler(object[] data)
        {
            int x = (int)data[0];
            int y = (int)data[1];
            int item = (int)data[2];

            CreateItem(x, y, item);
        }

        private void RemoveItem(int x, int y)
        {
            Destroy(objects[x][y]);
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
    }
}