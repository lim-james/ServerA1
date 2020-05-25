using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager
{
    private static AccountManager instance = null;

    public string username { get; private set; } 
    public List<string> friends { get; private set;  }

    public string chatName { get; private set; }
    public List<string> chat { get; private set;  }

    AccountManager()
    {
        friends = new List<string>();
    }

    public static AccountManager Instance()
    {
        if (instance == null)
            instance = new AccountManager();

        return instance;
    }

    public bool Login(string username, string rawPassword, out string error)
    {
        string hashedPassword = Hash(rawPassword);
        
        if (hashedPassword == Hash("password"))
        {
            this.username = username;
            error = "";
            return true;
        }
        else
        {
            error = "Wrong password.";
            return false;
        }
    }

    public bool Signup(string username, string rawPassword, out string error)
    {
        error = "";
        this.username = username;
        return true;
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
