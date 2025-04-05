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
        using var upgradeValue = UnityWebRequest.Get(GetCSVUrl(API.SheetURL, API.UpgradeValue));
        await upgradeValue.SendWebRequest();
        
        using var upgradeCost = UnityWebRequest.Get(GetCSVUrl(API.SheetURL, API.UpgradeCost));
        await upgradeCost.SendWebRequest();

        if (upgradeValue.result != UnityWebRequest.Result.Success ||
            upgradeCost.result != UnityWebRequest.Result.Success)
        {
            $"Error : {upgradeValue.error} \n{upgradeCost.error}".Log();
            return null;
        }

        var upValueCSV = ParseCSV(upgradeValue.downloadHandler.text);
        var upCostCSV = ParseCSV(upgradeCost.downloadHandler.text);
        var dic = new Dictionary<PlayFabEx.StatusType, List<StatusUpgrade>>();
        foreach (PlayFabEx.StatusType statusType in Enum.GetValues(typeof(PlayFabEx.StatusType)))
        {
            dic.Add(statusType, new List<StatusUpgrade>());

            for (int i = 0; i < upValueCSV[statusType].Count; ++i)
            {
                dic[statusType].Add(new StatusUpgrade(upValueCSV[statusType][i], (int)upCostCSV[statusType][i]));
            }
        }
        return dic;
    }

    private static Dictionary<PlayFabEx.StatusType, List<float>> ParseCSV(string csv)
    {
        csv = csv.Replace("\"", "");
        var dic = new Dictionary<PlayFabEx.StatusType, List<float>>();
        var lines = csv.Split('\n');
        foreach (var str in lines)
        {
            var values = str.Split(',');
            if (Enum.TryParse<PlayFabEx.StatusType>(values[0], out var statusType))
            {
                dic.Add(statusType, new List<float>( ));
                for (int i = 1; i < values.Length; ++i)
                {
                    if (string.Empty == values[i])
                        break;
                    dic[statusType].Add(float.Parse(values[i]));
                }
            }
            else
                Debug.LogWarning(values[0]);
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
    
}
