using System.Collections.Generic;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class GameManager : SerializedMonoBehaviour
{
    public void Awake()
    {
        if (null != I)
            Destroy(I.gameObject);
        I = this;
    }

    public static GameManager I;
    [OdinSerialize] public Dictionary<PlayableChar, GameObject> playableChar = new ();

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
