using System.IO;
using UnityEngine;

public class FileContorl
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path_with_name">Application.dataPath下的相对路径</param>
    /// <param name="data"></param>
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
}
