using UnityEditor;
using UnityEngine;

public static class LoadHelper
{
    private const string UIPath = "Assets/0.UI/";
    private const string UpGradeIcon = "UpGradeIcon/";
    private const string Portrait = "Portrait/";
    public static T LoadUpGradeIcon<T>(string name) where T : Object
    {
        return Load<T>($"{UIPath}{UpGradeIcon}", name);
    }
    
    public static T LoadPortrait<T>(string name) where T : Object
    {
        return Load<T>($"{UIPath}{Portrait}", name);
    }
    private static T Load<T>(string folderPath, string fileName) where T : Object
    {
        var path = $"{folderPath}{fileName}.png";
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}