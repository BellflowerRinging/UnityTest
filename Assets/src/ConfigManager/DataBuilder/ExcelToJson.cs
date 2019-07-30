using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExcelToJson : ScriptableObject
{

    [MenuItem("Tools/Excel To Json")]
    public static void Do()
    {
        Debug.Log("正在读取Excel");
        var table_list = ExcelDataManager.GetAllTable();

        Debug.Log("正在写出Json");
        JsonDataManager.WriteAllTabelToJsonFile(table_list);

        Debug.Log("转换完成");
    }

    [MenuItem("Tools/Create Data Struct")]
    public static void Do2()
    {
        Debug.Log("读取所有Json数据");
        var json_list = JsonDataManager.ReadTableList();

        Debug.Log("正在创建Struct");
        StructBuilder.CreateDataStruct(json_list);

        Debug.Log("正在创建ConfigReader");
        ReaderBuilder.CreateConfigReader(json_list);

        Debug.Log("创建完毕");
    }
}
