using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class DummyTest : MonoBehaviour
{

    private readonly string R = "A2:D";
    private readonly string sheet = "A2:D";
    private async void Start()
    {
        // var asd = UnityWebRequest.Get()
    }

    public static string Get(string address, string range, long sheetID)
    {
        return $"{address}/export?format=tsv&range={range}&fid={sheetID}";
    }

}
