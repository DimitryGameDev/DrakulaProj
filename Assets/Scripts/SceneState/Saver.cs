using System;
using System.IO;
using UnityEngine;

[Serializable]
public  class Saver<T>
{ 
    public T data;
    public static void TryLoad(string fileName, ref T data)
    {
        var path = FileHandler.Path(fileName);
        if (File.Exists(path))
        {
            Debug.Log($"Load file from :{FileHandler.Path(fileName)}");
            var dataString = File.ReadAllText(path);
            var saver = JsonUtility.FromJson<Saver<T>>(dataString);
            data = saver.data;
        }
        else
        {
            Debug.Log($"The file was not found :{FileHandler.Path(fileName)}");
        }
    }

    public static void Save(string fileName, T data)
    {
        //Debug.Log($"File saving to :{FileHandler.Path(fileName)}");
        var wrapper = new Saver<T>{data = data};
        var dataString = JsonUtility.ToJson(wrapper);
        File.WriteAllText(FileHandler.Path(fileName), dataString);
    } 
}

public static class FileHandler
{
    public static string Path(string fileName)
    {
        return $"{Application.persistentDataPath}/{fileName}";
    }

    public static void Reset(string fileName)
    {
        var path = Path(fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool HasFile(string fileName)
    {
        return File.Exists(Path(fileName));
    }
}