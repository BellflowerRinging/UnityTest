using System.Collections.Generic;

public static class ConfigManager
{
    static Dictionary<string, Dictionary<string, IConfigData>> m_config_data;

    public static void LoadAllConfig()
    {
        m_config_data = ConfigReader.ReadConfig();
    }

    /// <summary>
    /// 获得单个配置
    /// </summary>
    /// <typeparam name="T">配置结构体</typeparam>
    /// <param name="id">Id</param>
    /// <returns></returns>
    public static T GetConfig<T>(string id) where T : IConfigData
    {
        foreach (var cfg in m_config_data)
        {
            if (cfg.Key == typeof(T).Name)
            {
                foreach (var data in cfg.Value)
                {
                    if (data.Key == id)
                    {
                        return data.Value.ConvertTo<T>();
                    }
                }
            }
        }

        return default(T);
    }

    /// <summary>
    /// 获得所有配置
    /// </summary>
    /// <typeparam name="T">配置结构体</typeparam>
    /// <returns></returns>
    public static Dictionary<string, T> GetConfig<T>() where T : IConfigData
    {
        foreach (var cfg in m_config_data)
        {
            if (cfg.Key == typeof(T).Name)
            {
                return ConvertToIConfigData<T>(cfg.Value);
            }
        }

        return null;
    }

    private static Dictionary<string, T> ConvertToIConfigData<T>(Dictionary<string, IConfigData> value) where T : IConfigData
    {
        if (value == null) return null;

        var config = new Dictionary<string, T>();

        foreach (var cfg in value)
        {
            config.Add(cfg.Key, cfg.Value.ConvertTo<T>());
        }

        return config;
    }
}