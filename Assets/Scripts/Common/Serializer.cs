using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Serializer
{

    public static string ToString<T>(this T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            new BinaryFormatter().Serialize(stream, obj);
            return Convert.ToBase64String(stream.ToArray());
        }
    }

    public static T ToObject<T>(string raw)
    {
        byte[] bytes = Convert.FromBase64String(raw);
        using (MemoryStream stream = new MemoryStream(bytes, 0, bytes.Length))
        {
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            return (T)new BinaryFormatter().Deserialize(stream);
        }
    }

}

