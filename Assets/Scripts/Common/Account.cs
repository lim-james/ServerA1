using System;

[Serializable]
public struct Account
{
    public string name;
    public string password;
    
    public Account(string name, string password)
    {
        this.name = name;
        this.password = password;
    }
}

[Serializable]
public struct AccountResponse
{
    public bool success;
    public string message;
}
