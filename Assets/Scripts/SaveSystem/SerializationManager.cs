using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SerializationManager
{
    public static bool Save(object saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/levelState.save";
        FileStream file = File.Create(path);
        try
        {
            formatter.Serialize(file, saveData);
            file.Close();
            return true;
        }
        catch
        {
            file.Close();
            return false;
        }
    }

    public static object Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/saves/levelState.save"))
        {
            Debug.LogError("This path does not exist");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/saves/levelState.save", FileMode.Open);
        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogError("Failed to load the save");
            file.Close();
            return null;
        }
    }
}
