using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public class TestTools : ScriptableObject
{
    [MenuItem("Tools/这是命运石之门的选择")]
    static void DoIt()
    {
        //TestReadEXCEL();
        TestReadJsonTabel();
        /*
        var list = new List<StructBuilder.Property>();

        list.Add(new StructBuilder.Property("string", "str"));
        list.Add(new StructBuilder.Property("int", "i"));
        list.Add(new StructBuilder.Property("float", "f"));

        Debug.Log(StructBuilder.CreateStruct("Test", list, null, new StructBuilder.Config()));

        int i = 0;*/
    }

    private static void TestReadJsonTabel()
    {
        var tabel = JsonDataManager.ReadTabelData("BuffAttr");
    }

    private static void TestReadEXCEL()
    {
        var list = ExcelDataManager.GetTableList("Attritube");
        foreach (var tabel in list)
        {
            Debug.Log("table name:" + tabel.Name);

            Debug.Log("column:" + tabel.Name);

            foreach (var item in tabel.ColumnInfos)
            {
                Debug.Log(" name:" + item.Name + " Type:" + item.Type);
            }

            Debug.Log("row:" + tabel.Name);

            foreach (var item in tabel.RowDic)
            {
                Debug.Log(" Row Id:" + item.Key);

                foreach (var grid in item.Value)
                {
                    Debug.Log("     " + grid.Key + ":" + grid.Value);
                }
            }

            //var str = JsonConvert.SerializeObject(tabel);

            //StructBuilder.CreateDataStruct(tabel);

            //Debug.Log(str);

            JsonDataManager.WriteTabelToJsonFile(tabel);
        }

    }
}