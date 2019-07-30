using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public static class JsonDataManager
{
    const string PATH = "Resources/Json/";
    const string RESOURCE_PATH = "Json/";

    public static void WriteAllTabelToJsonFile(List<TabelData> datas)
    {
        Debug.Log(PATH);

        foreach (var item in datas)
        {
            Debug.Log("写出Json：" + item.Name);
            WriteTabelToJsonFile(item);
        }
    }


    /// <param name="path">Resources文件下</param>
    public static void WriteTabelToJsonFile(TabelData data)
    {
        FileContorl.WriteStringToFile(PATH + data.Name + ".json", JsonConvert.SerializeObject(data));
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
