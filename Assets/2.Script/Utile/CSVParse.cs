using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public static class CSVParse
{
    private static string GetCSVUrl(string api, string sheetName)
    {
        return$"https://docs.google.com/spreadsheets/d/{api}/gviz/tq?tqx=out:csv&sheet={sheetName}";
    }
    public static async Task<Dictionary<PlayFabEx.StatusType, List<StatusUpgrade>>> ReadCSV()
    {
        using var sheet1 = UnityWebRequest.Get(GetCSVUrl(API.GoogleSheetAPI, API.Sheet));
        await sheet1.SendWebRequest();
        
        using var sheet2 = UnityWebRequest.Get(GetCSVUrl(API.GoogleSheetAPI, API.Sheet));
        await sheet2.SendWebRequest();

        if (sheet1.result == UnityWebRequest.Result.Success && sheet2.result == UnityWebRequest.Result.Success)
            return ParseCSVToStatusUpgrade(sheet1.downloadHandler.text, sheet2.downloadHandler.text);
        
        $"Error : {sheet1.error} \n{sheet2.error}".Log();
        return null;
    }

    private static Dictionary<PlayFabEx.StatusType, List<StatusUpgrade>> ParseCSVToStatusUpgrade(string csv1, string csv2)
    {
        var dic = new Dictionary<PlayFabEx.StatusType, List<StatusUpgrade>>();
        var lines = csv1.Split('\n');
        var lines2 = csv2.Split('\n');
        for (int y = 0; y < lines.Length; ++y)
        {
            var values = lines[y].Split(',');
            var values2 = lines2[y].Split(',');
            
            
            var curType = PlayFabEx.StatusType.MaxHP;
            if (Enum.TryParse<PlayFabEx.StatusType>(values[0].Trim('\"'), out var statusType))
            {
                dic.Add(statusType, new List<StatusUpgrade>( ));
                curType = statusType;
            }
            else
            {
                Debug.LogWarning(values[0].Trim('\"'));
            }
            

            
            for (int i = 1; i < values.Length; ++i)
            {
                var val = values[i].Trim('\"');
                var goldCost = values2[i].Trim('\"');
                if(string.Empty ==val)
                    break;
                var statusUpgrade = new StatusUpgrade(float.Parse(val), int.Parse(goldCost));
                dic[curType].Add(statusUpgrade);
            }
        }
        return dic;
    }

    public struct StatusUpgrade
    {
        public StatusUpgrade(float val, int goldCost)
        {
            value = val;
            this.goldCost = goldCost;
        }
        public float value;
        public int goldCost;
    }

    private static void Test2()
    {
        var dic = new Dictionary<PlayFabEx.StatusType, List<StatusUpgrade>>();
        foreach (var status in Enum.GetValues(typeof(PlayFabEx.StatusType)))
        {
            dic.Add(Enum.Parse<PlayFabEx.StatusType>(status.ToString()), new List<StatusUpgrade>());
        }
    }

}
