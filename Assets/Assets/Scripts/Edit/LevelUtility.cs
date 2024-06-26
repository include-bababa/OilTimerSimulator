using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LevelUtility
{
    public static string SaveDir => Application.persistentDataPath;

    public static List<LevelData> LoadAll()
    {
        var ret = new List<LevelData>();

        var directoryInfo = new DirectoryInfo(SaveDir);
        var fileInfo = directoryInfo.GetFiles("*.json");
        foreach (var info in fileInfo)
        {
            var filename = info.Name;
            var data = Load(filename);
            if (data != null)
            {
                ret.Add(data);
            }
        }

        return ret.OrderByDescending(x => x.LastUpdate).ToList();
    }

    public static LevelData Load(string filename)
    {
        var path = Path.Join(SaveDir, filename);

        try
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var str = reader.ReadToEnd();
                var ret = JsonUtility.FromJson<LevelData>(str);
                ret.FileName = filename;
                Logger.Log($"Loaded: {path}");
                return ret;
            }
        }
        catch
        {
            return null;
        }
    }

    public static bool Save(LevelData data)
    {
        var json = JsonUtility.ToJson(data);

        var path = Path.Join(SaveDir, data.FileName);
        try
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }

            Logger.Log($"Saved: {path}");
        }
        catch
        {
            Logger.Log($"Save failed: {path}");
            return false;
        }

        return true;
    }

    public static bool Delete(LevelData data)
    {
        var path = Path.Join(SaveDir, data.FileName);
        try
        {
            File.Delete(path);

            Logger.Log($"Deleted: {path}");
        }
        catch
        {
            return false;
        }

        return true;
    }
}
