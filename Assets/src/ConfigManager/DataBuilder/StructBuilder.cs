using System;
using System.Collections.Generic;
using System.Text;

public struct AccessModifierConst
{
    public const string PUBLIC = "public";
    public const string PRIVATE = "private";
    public const string PROTECTED = "protected";
}

public class StructBuilder
{
    private static readonly string PATH = "src/ConfigManager/DataStruct/";

    public struct Property
    {
        public string AccessModifier;
        public string SetAccessModifier;
        public string Type;
        public string Name;

        public Property(string type, string name,
            string accessModifier = AccessModifierConst.PUBLIC,
            string setAccessModifier = AccessModifierConst.PUBLIC)
        {
            AccessModifier = accessModifier;
            SetAccessModifier = setAccessModifier;
            Type = type;
            Name = name;
        }
    }

    public struct Method
    {
        public string AccessModifier;
        public bool IsStatic;
        public string ReturnType;
        public string Name;
        public Dictionary<string, string> Parameters;

        public Method(string returnType, string name, Dictionary<string, string> parameters = null, string accessModifier = AccessModifierConst.PUBLIC, bool isStatic = false)
        {
            AccessModifier = accessModifier;
            IsStatic = isStatic;
            ReturnType = returnType;
            Name = name;
            Parameters = parameters;
        }
    }

    public struct Config
    {
        List<string> UsingList;
    }

    public static void CreateDataStruct(TabelData data)
    {
        CreateDataStruct(data.Name, data.ColumnInfos);
    }

    public static void CreateDataStruct(string name, List<ColumnInfo> infos)
    {
        var prop_list = new List<Property>();

        foreach (var item in infos)
        {
            prop_list.Add(new Property(item.Type, item.Name));
        }

        var result = CreateStruct(name, prop_list, null, new Config());

        JsonDataManager.WriteStringToFile(PATH + name + ".cs", result);
    }

    public static string CreateStruct(string name, List<Property> prop_list, List<Method> method_list, Config congif)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("public struct ").Append(name).Append(" : IConfigData\n{\n");

        if (prop_list != null)
        {
            foreach (var prop in prop_list)
            {
                builder.Append(CreateProperty(prop)).Append("\n");
            }
        }

        builder.Append(CreateConstructor(name, prop_list));

        if (method_list != null)
        {
            foreach (var prop in method_list)
            {
                builder.Append(CreateMethod(prop)).Append("\n");
            }
        }

        builder.Append("}\n");

        return builder.ToString();
    }

    private static string CreateProperty(Property prop)
    {
        if (prop.SetAccessModifier == null || prop.SetAccessModifier == AccessModifierConst.PUBLIC)
        {
            prop.SetAccessModifier = "";
        }
        else
        {
            prop.SetAccessModifier += " ";
        }

        return string.Format("{0} {1} {2} {{ get; {3}set;}}", prop.AccessModifier, prop.Type, prop.Name, prop.SetAccessModifier);
    }

    private static string CreateConstructor(string name, List<Property> prop_list)
    {
        var builder = new StringBuilder();

        builder.Append("public ").Append(name).Append("(");

        if (prop_list != null)
        {
            for (int i = 0; i < prop_list.Count; i++)
            {
                var prop = prop_list[i];

                builder.Append(string.Format("{0} {1}", prop.Type, prop.Name));

                if (i < prop_list.Count - 1) builder.Append(",");
            }

            builder.Append(")\n{\n");

            foreach (var prop in prop_list)
            {
                builder.Append("this.").Append(prop.Name).Append(" = ").Append(prop.Name).Append(";\n");
            }

            builder.Append("}\n");
        }

        return builder.ToString();
    }

    private static string CreateMethod(Method prop)
    {
        return "";
    }


}

