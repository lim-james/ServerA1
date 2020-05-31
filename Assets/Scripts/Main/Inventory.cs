using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Inventory : MonoBehaviour
    {

        [SerializeField]
        private InventoryManager manager;

        private PlayerMovement movement;

        public int size { get; private set; }
        public int index { get; private set; }
        public int[] items { get; private set; }

        private string username;

        private void Awake()
        {
            size = 4;
            movement = GetComponent<PlayerMovement>();

            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        private void Start()
        {
            index = 0;
            username = AccountManager.Instance().username;

            items = new int[size];
            for (int i = 0; i < size; ++i)
                items[i] = -1;

            PhotonNetwork.RaiseEvent((int)Events.GET_INVENTORY, username, RaiseEventOptions.Default, SendOptions.SendReliable);
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
                Place();
        }

        private void OnEvent(EventData obj)
        {
            Events e = (Events)obj.Code;
            
            if (e == Events.GET_INVENTORY)
                InventoryHandler((object[])obj.CustomData);
            else if (e == Events.PICKUP_ITEM)
                PickupHandler((object[])obj.CustomData);
            else if (e == Events.PLACE_ITEM)
                PlaceHandler((object[])obj.CustomData);
        }

        private void InventoryHandler(object[] data)
        {
            for (int i = 0; i < size; ++i)
                items[i] = (Int32)data[i];
            manager.ReloadData();
        }

        private void Pickup()
        {
            if (items[index] >= 0)
                return;
            
            Vector3 target = transform.position + movement.direction;

            object[] data = new object[] {
                username,
                index,
                (int)target.x,
                (int)target.y
            };

            PhotonNetwork.RaiseEvent((int)Events.PICKUP_ITEM, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private void PickupHandler(object[] data)
        {
            bool succcess = (bool)data[0];
            int i = (int)data[1];
            int itemId = (int)data[2];
            
            if (succcess)
            {
                items[i] = itemId;
                manager.ReloadData();
            }
        }

        private void Place()
        {
            if (items[index] < 0)
                return;
            
            Vector3 target = transform.position + movement.direction;
            
            object[] data = new object[] {
                username,
                index,
                items[index],
                (int)target.x,
                (int)target.y
            };

            PhotonNetwork.RaiseEvent((int)Events.PLACE_ITEM, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        private void PlaceHandler(object[] data)
        {
            bool succcess = (bool)data[0];
            int index = (int)data[1];

            if (succcess)
            {
                items[index] = -1;
                manager.ReloadData();
            }
        }
        
    }
}
