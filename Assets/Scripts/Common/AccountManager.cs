using System;
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
            // TODO - Send to db for authentication
            // SELECT password FROM Socials.Accounts WHERE username='james';
            String sqlCmd = String.Format("SELECT password FROM Socials.Accounts WHERE username='{0}';", username);
            Debug.Log(sqlCmd);

            string result = "{REPLACE THIS}";

            if (result == hashedPassword) {
                error = "";
                return true;
            } else {
                error = "Wrong password.";
                return false;
            }
        }
    }

    public bool Signup(string username, string rawPassword, out string error)
    {
        error = "";
        this.username = username;
        string hashedPassword = Hash(rawPassword);

        // TODO - Send to db for authentication

        // Create account 
        // INSERT INTO Socials.Accounts(username, password) VALUES ('username', 'password');
        String sqlCmd = String.Format("INSERT INTO Socials.Accounts(username, password) VALUES ('{0}', '{1}');", username, hashedPassword);
        Debug.Log(sqlCmd);

        // Create friend table
        sqlCmd = String.Format(@"
            CREATE TABLE Socials.`Friends_{0}` (
            `index` INT NOT NULL AUTO_INCREMENT,
            `username` VARCHAR(21) NOT NULL,
            PRIMARY KEY (`index`),
            UNIQUE INDEX index_UNIQUE (`index` ASC) VISIBLE);", 
            username
        );

        Debug.Log(sqlCmd);
        
        // INSERT INTO Main.Inventory(username) VALUES ('username');
        sqlCmd = String.Format("INSERT INTO Main.Inventory(username) VALUES ('{0}');", username);
        Debug.Log(sqlCmd);
        // INSERT INTO Main.Positions(username, x, y) VALUES ('username', 0, 0);
        sqlCmd = String.Format("INSERT INTO Main.Positions(username, x, y) VALUES ('{0}', 0, 0);", username);
        Debug.Log(sqlCmd);
        
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
