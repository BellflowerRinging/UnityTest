using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class ExcelDataManager
{
    static readonly string PATH = Application.dataPath + "/Excel/";

    const int INTRODUCE_ROW = 0; //注释行
    const int COLUNM_NAME_ROW = 1; //列名行
    const int COLUNM_TYPE_ROW = 2; //列类型行
    const int DATA_START_TOW = 3;  //数据行从这里开始读

    public static List<TabelData> GetAllTable()
    {
        var list = new List<TabelData>();
        var names = Directory.GetFiles(PATH);

        Debug.Log(PATH);

        foreach (var item in names)
        {
            if (Path.GetExtension(item) != ".xlsx") continue;

            list.AddRange(GetTableList(Path.GetFileNameWithoutExtension(item)));
        }

        return list;
    }

    public static List<TabelData> GetTableList(string ExcelName)
    {
        ExcelName = PATH + ExcelName + ".xlsx";

        if (!File.Exists(ExcelName)) throw new UnityException("File is Not:" + ExcelName);
        FileStream stream = File.Open(ExcelName, FileMode.Open, FileAccess.Read);

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        var table_list = new List<TabelData>();

        foreach (DataTable table in result.Tables)
        {
            var tabel_data = new TabelData();

            tabel_data.Name = table.TableName;

            Debug.Log(ExcelName + " - " + tabel_data.Name);

            tabel_data.ColumnInfos = GetColunmInfos(table.Rows[COLUNM_NAME_ROW], table.Rows[COLUNM_TYPE_ROW]);
            tabel_data.RowDic = GetRowDataList(tabel_data.ColumnInfos, table.Rows);

            table_list.Add(tabel_data);
        }

        stream.Close();

        return table_list;
    }

    public static List<ColumnInfo> GetColunmInfos(DataRow name_row, DataRow type_row)
    {
        var list = new List<ColumnInfo>();

        for (int i = 0; i < name_row.ItemArray.Length; i++)
        {
            var name = name_row.ItemArray[i].ToString();
            var type = type_row.ItemArray[i].ToString();

            if (string.IsNullOrEmpty(name)) continue;

            if (string.IsNullOrEmpty(type)) throw new UnityException("Type == Null");

            list.Add(new ColumnInfo()
            {
                Name = name,
                Type = type
            });
        }

        return list;
    }

    public static Dictionary<string, Dictionary<string, object>> GetRowDataList(List<ColumnInfo> columnInfos, DataRowCollection rows)
    {
        var list = new Dictionary<string, Dictionary<string, object>>();

        for (int i = DATA_START_TOW; i < rows.Count; i++)
        {
            DataRow row = rows[i];

            var id = row[0].ToString();

            if (string.IsNullOrEmpty(id)) continue;

            list.Add(id, GetGridDic(columnInfos, row));
        }

        return list;
    }

    public static Dictionary<string, object> GetGridDic(List<ColumnInfo> columnInfos, DataRow row)
    {
        var list = new Dictionary<string, object>();

        for (int i = 0; i < columnInfos.Count; i++)
        {
            var column = columnInfos[i];

            list.Add(column.Name, BuildGridValue(column.Type, row[i]));
        }

        return list;
    }

    private static object BuildGridValue(string valueType, object v)
    {
        switch (valueType)
        {
            case "string": if (ObjIsNullOrEmpty(v)) return ""; return v.ToString();
            case "int": if (ObjIsNullOrEmpty(v)) return 0; return Convert.ToInt32(v);
            case "float": if (ObjIsNullOrEmpty(v)) return 0f; return Convert.ToSingle(v);
            case "bool": if (ObjIsNullOrEmpty(v)) return false; return Convert.ToInt32(v) == 0 ? false : true;
            default: return null;
        }
    }

    static bool ObjIsNullOrEmpty(object obj)
    {
        return obj == null || obj.ToString() == "";
    }
}

public struct TabelData
{
    public string Name;
    public List<ColumnInfo> ColumnInfos; //列名-类型
    public Dictionary<string, Dictionary<string, object>> RowDic; //id key-value
}

public struct RowData
{
    public string Id;
    public Dictionary<string, object> GridDic;
}

public struct GridData
{
    public string Key;
    public object Value;
}

public struct ColumnInfo
{
    public string Name;
    public string Type;
}