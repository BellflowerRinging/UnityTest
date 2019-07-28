using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public static class JsonDataManager
{
    const string PATH = "Resources/Json/";
    const string RESOURCE_PATH = "Json/";
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path">Resources文件下</param>
    public static void WriteTabelToJsonFile(TabelData data)
    {
        WriteStringToFile(PATH + data.Name + ".json", JsonConvert.SerializeObject(data));
    }

    public static void WriteStringToFile(string path_with_name, string data)
    {
        var resouece_path = Application.dataPath + "/" + path_with_name;

        FileStream file_stream;
        if (!File.Exists(resouece_path))
        {
            file_stream = File.Create(resouece_path);
        }
        else file_stream = File.OpenWrite(resouece_path);

        var stream = new StreamWriter(file_stream);

        stream.Write(data);

        stream.Close();
    }

    public static List<TabelData> ReadTableList()
    {
        var list = new List<TabelData>();
        var names = Directory.GetFiles(Application.dataPath + "/" + PATH);

        foreach (var item in names)
        {
            if (Path.GetExtension(item) != ".json") continue;

            list.Add(ReadTabelData(Path.GetFileNameWithoutExtension(item)));
        }

        return list;
    }

    public static TabelData ReadTabelData(string json_name)
    {
        return TransfromToTabelData(ReadJsonData(json_name));
    }

    private static string ReadJsonData(string json_name, bool with_path = false)
    {
        Debug.Log("ReadJsonData - " + json_name);

        var data = Resources.Load(RESOURCE_PATH + json_name).ToString();

        return data;
    }

    private static TabelData TransfromToTabelData(string json_data)
    {
        var tabel_data = JsonConvert.DeserializeObject<TabelData>(json_data);

        return tabel_data;
    }

    public static string GetString(this Dictionary<string, object> dic, string key)
    {
        return dic[key].ToString();
    }

    public static float GetFloat(this Dictionary<string, object> dic, string key)
    {
        return Convert.ToSingle(dic[key]);
    }
    public static float GetInt(this Dictionary<string, object> dic, string key)
    {
        return Convert.ToInt32(dic[key]);
    }
    public static bool GetBool(this Dictionary<string, object> dic, string key)
    {
        return (bool)dic[key];
    }
}
