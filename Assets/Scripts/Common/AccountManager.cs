using System;
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
                AccountResponse response = Serializer.ToObject<AccountResponse>((string)obj.CustomData);
                if (!response.success) username = "";
                loginHandler.Invoke(response.success, response.message); 
            }
        }

        public void Login(string username, string rawPassword)
        {
            this.username = username;
            Account account = new Account(username, Hash(rawPassword));
            PhotonNetwork.RaiseEvent((int)Events.LOGIN, Serializer.ToString(account), RaiseEventOptions.Default, SendOptions.SendReliable);
        }

        public bool AddUser(string name, out string error)
        {
            error = "";
            if (name == "")
                return true;

            if (name == username)
            {
                error = "Can't add yourself.";
                return false;
            }

            // TODO - Add user to friend list
            // INSERT INTO Socials.Friends_username(username) VALUES ('name');
            String sqlCmd = String.Format("INSERT INTO Socials.Friends_{0}(username) VALUES ('{1}');", username, name);
            Debug.Log(sqlCmd);

            Debug.Log("Adding " + name);
            friends.Add(name);
            return true;
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
