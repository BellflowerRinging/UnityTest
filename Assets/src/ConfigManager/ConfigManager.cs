using System.Collections.Generic;

public static class ConfigManager
{
    static Dictionary<string, Dictionary<string, IConfigData>> m_config_data;

    public static void LoadAllConfig()
    {
        //m_config_data = ConfigReader.ReadConfig();
    }

    public static T GetConfig<T>(string config, string id) where T : IConfigData
    {
        foreach (var cfg in m_config_data)
        {
            if (cfg.Key == config)
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

    public static Dictionary<string, T> GetConfig<T>(string config) where T : IConfigData
    {
        foreach (var cfg in m_config_data)
        {
            if (cfg.Key == config)
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