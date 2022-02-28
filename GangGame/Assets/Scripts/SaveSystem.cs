using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    private static string path = Application.persistentDataPath + "/savetest";

    public static void SaveData (GameManager gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Data data = new Data(gm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data LoadData()
    {
        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        Data data = formatter.Deserialize(stream) as Data;
        stream.Close();

        return data;
    }
}
