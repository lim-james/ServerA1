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
public struct FriendRequest
{
    public string sender;
    public string name;

    public FriendRequest(string sender, string name)
    {
        this.sender = sender;
        this.name = name;
    }
}

[Serializable]
public struct Response
{
    public bool success;
    public string message;
}
