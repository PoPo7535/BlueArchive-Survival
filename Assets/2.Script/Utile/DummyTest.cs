using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class DummyTest : MonoBehaviour
{

    private string csvUrl = $"https://docs.google.com/spreadsheets/d/{API.GoogleSheetAPI}/gviz/tq?tqx=out:csv";

    void Start()
    {
        StartCoroutine(ReadCSV());
    }
    
    [Button]
    IEnumerator ReadCSV()
    {
        using UnityWebRequest www = UnityWebRequest.Get(csvUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string csvData = www.downloadHandler.text;
            Debug.Log("Received CSV: " + csvData);
        }
        else
        {
            Debug.LogError("Error: " + www.error);
        }
    }

}
