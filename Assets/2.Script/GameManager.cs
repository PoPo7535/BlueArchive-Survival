using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

public class GameManager : SerializedMonoBehaviour, ISetInspector
{
    public static GameManager I;
    [OdinSerialize] public Dictionary<PlayableChar, GameObject> playableChar = new ();
   
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
#if UNITY_EDITOR   
        var arr = PlayableChar.None.ToArray();
        playableChar = new Dictionary<PlayableChar, GameObject>();
        foreach (var playable in arr.Skip(1))
        {
            var root = $"Assets/5.Prefab/Playable Character/{playable.ToString()}.prefab";
            root.Log();
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(root);
            playableChar.Add(playable, obj);
        }
        GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
        PrefabUtility.ApplyPrefabInstance(prefabRoot , InteractionMode.UserAction);
#endif
    }
    public void Awake()
    {
        if (null != I)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        I = this;
    }


    public GameObject player;
    public GameObject kayoko;
    public GameObject kayoko2;

    public PlayerInfo playerInfo;
    public EntityTokenResponse playFabEntity;
    [ReadOnly] public string ID;
    
}

public enum PlayableChar
{
    None,
    Kayoko,
    Neru,
    Miyu,
    Hoshino,
    Aris,
}
