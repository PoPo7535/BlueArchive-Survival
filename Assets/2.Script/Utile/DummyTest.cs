using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class DummyTest : MonoBehaviour
{

    private static string csvUrl = $"https://docs.google.com/spreadsheets/d/{API.GoogleSheetAPI}/gviz/tq?tqx=out:csv";


    
    [Button]
    private static async void ReadCSV()
    {
        using var www = UnityWebRequest.Get(csvUrl);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var csvData = www.downloadHandler.text;
            csvData.Log();
            ParseCSV(csvData);
        }
        else
        {
            Debug.LogError("Error: " + www.error);
        }
    }

    private static void ParseCSV(string csv, int x = 1, int y = 1)
    {
        var lines = csv.Split('\n');
        lines.Length.Log();
        foreach (var line in lines.Skip(y))
        {
            var values = line.Split(',');
            values.Length.Log();
            foreach (var val in values.Skip(x))
            {
                val.Log();
            }
        }
    }

}
