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
                Drop();
        }

        private void OnEvent(EventData obj)
        {
            Events e = (Events)obj.Code;

            if (e == Events.GET_INVENTORY)
                InventoryHandler((object[])obj.CustomData);
            else if (e == Events.PICKUP_ITEM)
                PickupHandler((object[])obj.CustomData);
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

            Debug.Log("Picking up");
            Vector3 target = transform.position + movement.direction;
            grid.PickItem(username, index, target);
        }

        private void PickupHandler(object[] data)
        {
            bool succcess = (bool)data[0];
            int i = (int)data[1];
            int itemId = (int)data[2];

            Debug.Log(String.Format("Picked up {0}, {1}, {2}", succcess, i, itemId));

            if (succcess)
            {
                items[i] = itemId;
                manager.ReloadData();
            }
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
}
