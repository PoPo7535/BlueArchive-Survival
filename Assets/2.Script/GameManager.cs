using PlayFab.ClientModels;
using UnityEngine;
using Utility;

public class GameManager : Singleton<GameManager>
{
    public GameObject player;

    public GameObject kayoko;
    public GameObject kayoko2;

    public PlayerInfo playerInfo;

    public EntityTokenResponse playFabEntity;
    public string ID;
}
