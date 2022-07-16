using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class GuardadoGenerico<T>
{
    public void Save(T obj, string fileName)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, obj);
        }
        catch (SerializationException se)
        {
            Debug.LogError("There was an issue Serializing this data: " + se.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public T Load(string fileName)
    {
        T stats = default;
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            file = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Open);
        }
        else return default;

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            stats = (T)formatter.Deserialize(file);
        }
        catch (SerializationException se)
        {
            Debug.LogError("There was an issue Deserializing this data: " + se.Message);
        }
        finally
        {
            file.Close();
        }
        return stats;
    }

    public void Delete(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            File.Delete(Application.persistentDataPath + "/" + fileName);
            Debug.Log(fileName + " deleted");
        }
        else Debug.Log("The file doesn't exist");
    }

    public bool Exists(string fileName)
    {
        return File.Exists(Application.persistentDataPath + "/" + fileName);
    }
}