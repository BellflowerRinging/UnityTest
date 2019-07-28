using System.Collections.Generic;

public static class ConfigReader
{
    public static Dictionary<string, Dictionary<string, IConfigData>> ReadConfig()
    {
        var config = new Dictionary<string, Dictionary<string, IConfigData>>();

        config.Add("BuffAttr", LoadBuffAttr());


        return config;
    }

    private static Dictionary<string, IConfigData> LoadBuffAttr()
    {
        var tabel = JsonDataManager.ReadTabelData("BuffAttr");
        var config = new Dictionary<string, IConfigData>();

        foreach (var kv in tabel.RowDic)
        {
            var row = kv.Value;

            var buff = new BuffAttr
            {
                Id = row.GetString("Id"),
                Name = row.GetString("Name"),
                ChinName = row.GetString("ChinName"),
                Introduce = row.GetString("Introduce"),
                Immediately = row.GetBool("Immediately"),
                Repeat = row.GetBool("Repeat"),
                Value_0 = row.GetFloat("Value_0"),
                Value_1 = row.GetFloat("Value_1"),
                Value_2 = row.GetFloat("Value_2")
            };

            config.Add(buff.Id, buff);
        }

        return config;
    }



}