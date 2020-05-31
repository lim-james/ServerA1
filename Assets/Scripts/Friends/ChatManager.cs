using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text header;
    [SerializeField]
    private ChatTableView table;
    [SerializeField]
    private InputField inputField;

    private string username;
    private int activeChat;
    private List<string> chat;

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
        username = AccountManager.Instance().username;
    }

    private void OnEvent(EventData obj)
    {
        Events e = (Events)obj.Code;

        if (e == Events.OPEN_CHAT)
        {
            object[] data = (object[])obj.CustomData;
            OpenHandler(data);
        }
        else if (e == Events.RECEIVE_CHAT)
        {
            object[] data = (object[])obj.CustomData;
            ReceiveHandler(data);
        }
    }

    public void Open(string user)
    {
        // TODO - Get chat data
        object[] data = new object[] { username, user };
        PhotonNetwork.RaiseEvent((int)Events.OPEN_CHAT, data, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Send()
    {
        object[] data = new object[] { activeChat, username, inputField.text };
        PhotonNetwork.RaiseEvent((int)Events.SEND_CHAT, data, RaiseEventOptions.Default, SendOptions.SendReliable);

        chat.Insert(0, String.Format("[{0}] {1}", username, inputField.text));
        inputField.text = "";
        table.ReloadData();
    }

    private void OpenHandler(object[] data)
    {
        activeChat = (Int32)data[0];
        string user = (string)data[1];
        int count = (Int32)data[2];

        panel.SetActive(true);
        header.text = user;

        chat = new List<string>();
        for (int i = 0; i < count; ++i)
        {
            int index = i * 3 + 3;
            string sender = (string)data[index];
            string message = (string)data[index + 1];
            string dateTime = (string)data[index + 2];

            chat.Insert(0, String.Format("[{0}] {1}", sender, message));
        }
        table.SetChat(chat);
    }

    private void ReceiveHandler(object[] data)
    {
        int index = (Int32)data[0];
        string sender = (string)data[1];
        string message = (string)data[2];

        if (index == activeChat && sender != username)
        {
            chat.Insert(0, String.Format("[{0}] {1}", sender, message));
            table.ReloadData();
        }
    }
}
