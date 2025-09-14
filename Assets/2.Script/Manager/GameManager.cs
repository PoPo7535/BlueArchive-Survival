using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using PlayFab.ClientModels;
using Fusion;
using TMPro;
using UnityEngine.Serialization;
#if UNITY_EDITOR   
using UnityEditor;
#endif
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
public class GameManager : SerializedMonoBehaviour, ISetInspector
{
    public static GameManager I;
    [OdinSerialize] public Dictionary<PlayableChar, GameObject> playableChar = new ();
    public Dictionary<SkillType, SkillScriptable> skillDataTests = new();
    public NetworkObject playerBase;
    public PlayerInfo playerInfo;
    public TMP_Text textUi;
    [NonSerialized]public EntityTokenResponse playFabEntity;
    [Read] public string ID;
    
    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
#if UNITY_EDITOR   
        playableChar.Clear();
        var playableChars = PlayableChar.None.ToArray();
        foreach (var playable in playableChars.Skip(1))
        {
            var root = $"Assets/5.Prefab/Playable Character/{playable.ToString()}.prefab";
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(root);
            playableChar.Add(playable, obj);
        }
        skillDataTests.Clear();
        var skillTypes = SkillType.None.ToArray();
        foreach (var skillType in skillTypes.Skip(1))
        {
            var root = $"Assets/3.ScriptableObject/{skillType.ToString()}.asset";
            var obj = AssetDatabase.LoadAssetAtPath<SkillScriptable>(root);
            obj._skillType = SkillType.Wheel;
            obj.SetSkillType();
            skillDataTests.Add(skillType, obj);
        }
        
        var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
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
