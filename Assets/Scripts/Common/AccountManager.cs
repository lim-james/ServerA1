﻿using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Photon.Pun
{
    public class AccountManager
    {

        private static AccountManager instance = null;

        public string username { get; private set; }
        public List<string> friends { get; private set; }

        public string chatName { get; private set; }
        public List<string> chat { get; private set; }

        public UnityAction<bool, string> loginHandler;
        public UnityAction<bool, string> addHandler;

        AccountManager()
        {
            friends = new List<string>();
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        public static AccountManager Instance()
        {
            if (instance == null)
                instance = new AccountManager();

            return instance;
        }

        private void OnEvent(EventData obj)
        {
            Events e = (Events)obj.Code;
            if (e == Events.LOGIN)
            {
                Debug.Log("Got login event");
                object[] data = (object[])obj.CustomData;
                bool success = (bool)data[0];
                string message = (string)data[1];
                if (!success) username = "";
                loginHandler.Invoke(success, message); 
            } 
            else if (e == Events.ADD_FRIEND)
            {
                object[] data = (object[])obj.CustomData;
                bool success = (bool)data[0];
                string message = (string)data[1];
                addHandler.Invoke(success, message); 
            }
        }

        public void Login(string username, string rawPassword)
        {
            this.username = username;
            object[] data = new object[] { username, Hash(rawPassword) };
            PhotonNetwork.RaiseEvent((int)Events.LOGIN, data, RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        public void AddUser(string name)
        {
            if (name == "")
            {
                addHandler.Invoke(true, "");
                return;
            }

            if (name == username)
            {
                addHandler.Invoke(false, "Can't add yourself.");
                return;
            }

            object[] data = new object[] { username, name };
            PhotonNetwork.RaiseEvent((int)Events.ADD_FRIEND, data, RaiseEventOptions.Default, SendOptions.SendReliable);

            Debug.Log("Adding " + name);
            friends.Add(name);
        }

        private string Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; ++i)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }
    }
}
