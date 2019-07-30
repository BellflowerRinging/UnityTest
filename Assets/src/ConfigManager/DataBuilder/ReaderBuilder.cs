using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public interface IConfigData : IMessage
{

}



public class ReaderBuilder : ScriptableObject
{
    private static readonly string PATH = "src/ConfigManager/AutoReadOnly/";

    /// <summary>
    /// $READER_METHOE$
    /// $LOAD_ROW_METHOE_LIST$
    /// </summary>
    private static readonly string READER_MODEL =

@"using System.Collections.Generic;

public static class ConfigReader
{
" + MACRO_READER_METHOE + @"

" + MACRO_LOAD_TABLE_METHOE_LIST + @"
}";

    /// <summary>
    /// 对应READER_METHOE模型
    /// </summary>
    const string MACRO_READER_METHOE = "$READER_METHOE$";

    /// <summary>
    /// 对应LOAD_TABLE_METHOE模型的list
    /// </summary>
    const string MACRO_LOAD_TABLE_METHOE_LIST = "$LOAD_TABLE_METHOE_LIST$";

    /// <summary>
    /// 读取所有配置的的方法
    /// </summary>
    private static readonly string READER_METHOE =
@"   public static Dictionary<string, Dictionary<string, IConfigData>> ReadConfig()
    {
        var config = new Dictionary<string, Dictionary<string, IConfigData>>();

        " + MACRO_CONFIG_ADD_LIST + @"

        return config;
    }";

    /// <summary>
    /// $CONFIG_ADD_LIST$ 读取表格名称存入字典的行list  -> CONFIG_ADD 的 list
    /// </summary>
    const string MACRO_CONFIG_ADD_LIST = "$CONFIG_ADD_LIST$";


    /// <summary>
    /// 读取单张表的数据 存入字典中
    /// </summary>
    private static readonly string CONFIG_ADD = @"config.Add(""" + MACRO_TABLE_NAME + @""", Load" + MACRO_TABLE_NAME + @"());";

    /// <summary>
    /// $TABLE_NAME$ 表名称
    /// </summary>
    const string MACRO_TABLE_NAME = "$TABLE_NAME$";

    /// <summary>
    /// 读取单张表的数据的方法
    /// $TABLE_NAME$ 表格名称
    /// $GET_PROP_LIST$ 数据存入结构体的行的列表 需要手动join “,” -> GET_PROP的list
    /// </summary>
    private static readonly string LOAD_TABLE_METHOE =
@"  private static Dictionary<string, IConfigData> Load" + MACRO_TABLE_NAME + @"()
    {
        var tabel = JsonDataManager.ReadTabelData(""" + MACRO_TABLE_NAME + @""");
        var config = new Dictionary<string, IConfigData>();

        foreach (var kv in tabel.RowDic)
        {
        var row = kv.Value;

        var buff = new " + MACRO_TABLE_NAME + @"
        {
            " + MACRO_GET_PROP_LIST + @"
        };

        config.Add(buff.Id, buff);
        }

        return config;
    }

";

    /// <summary>
    /// $GET_PROP_LIST$ 数据存入结构体的行的列表 需要手动join “,” -> GET_PROP的list
    /// </summary>
    const string MACRO_GET_PROP_LIST = "$GET_PROP_LIST$";


    /// <summary>
    /// 每行末尾需要，需要手动join
    /// </summary>
    private static readonly string GET_PROP = MACRO_PROP_NAME + @" = row.Get" + MACRO_Type + @"(""" + MACRO_PROP_NAME + @""")";

    /// <summary>
    /// $PROP_NAME$ 字段名称
    /// </summary>
    const string MACRO_PROP_NAME = "$PROP_NAME$";

    /// <summary>
    /// $Type$ 类型名称 首字母大写
    /// </summary>
    const string MACRO_Type = "$Type$";


    public static void CreateConfigReader(List<TabelData> table_list)
    {
        Debug.Log("创建ReaderConfig类");
        var reader = CreateReaderClass(table_list);

        Debug.Log("写出ReaderConfig类 " + PATH + "ConfigReader.cs");
        FileContorl.WriteStringToFile(PATH + "ConfigReader.cs", reader);

        Debug.Log("创建完成");
    }

    private static string CreateReaderClass(List<TabelData> table_list)
    {
        var cs = READER_MODEL;

        cs = cs.Replace(MACRO_READER_METHOE, CreateRederMethoe(table_list));
        cs = cs.Replace(MACRO_LOAD_TABLE_METHOE_LIST, CreateLoadTabelMethoeList(table_list));

        return cs;
    }

    private static string CreateRederMethoe(List<TabelData> table_list)
    {
        var cs = READER_METHOE;

        cs = cs.Replace(MACRO_CONFIG_ADD_LIST, CreateConfigAddList(table_list));

        return cs;
    }

    private static string CreateConfigAddList(List<TabelData> table_list)
    {
        var str_list = new StringBuilder();

        foreach (var item in table_list)
        {
            var cs = CONFIG_ADD + "\n";

            cs = cs.Replace(MACRO_TABLE_NAME, item.Name);

            str_list.Append(cs);
        }

        return str_list.ToString();
    }

    private static string CreateLoadTabelMethoeList(List<TabelData> tabel_list)
    {
        var str_list = new StringBuilder();

        foreach (var item in tabel_list)
        {
            str_list.Append(CreateLoadTabelMethoe(item)).Append("\n");
        }

        return str_list.ToString();
    }

    private static string CreateLoadTabelMethoe(TabelData tabel)
    {
        var cs = LOAD_TABLE_METHOE;

        cs = cs.Replace(MACRO_TABLE_NAME, tabel.Name);
        cs = cs.Replace(MACRO_GET_PROP_LIST, CreateGetPropList(tabel.ColumnInfos));

        return cs;
    }

    private static string CreateGetPropList(List<ColumnInfo> prop_list)
    {
        var list = new List<string>();

        foreach (var item in prop_list)
        {
            var cs = GET_PROP;

            cs = cs.Replace(MACRO_PROP_NAME, item.Name);
            cs = cs.Replace(MACRO_Type, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Type));

            list.Add(cs);
        }

        return string.Join(",\n", list.ToArray());
    }
}