using System.IO;
using UnityEngine;

public static class JsonLogger
{
    private static readonly bool prettyPrint = true;
    private static readonly string dataPath = Application.dataPath+"/JsonLogs/data.txt";
    public static void WriteJson(object o)
    {
        string jsonOutput = JsonUtility.ToJson(o,prettyPrint);
        File.WriteAllText(dataPath,jsonOutput);
    }

    public static void ClearJson() => File.Delete(dataPath);
}