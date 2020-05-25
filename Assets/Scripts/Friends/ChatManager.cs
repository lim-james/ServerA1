using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private List<string> chat;

    public void Open(string user)
    {
        panel.SetActive(true);
        header.text = user;

        chat = new List<string>();
        table.SetChat(chat);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Send()
    {
        chat.Insert(0, inputField.text);
        inputField.text = "";
        table.ReloadData();
    }

}
