using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;

public class QuickLogin : MonoBehaviour
{
    #if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(new Vector2(50, 50), new Vector2(300, 150)),
                "A"))
        {
            Login("dltngus7535", "5150156q");
        }

        if (GUI.Button(new Rect(new Vector2(50, 300), new Vector2(300, 150)),
                "B"))
        {
            Login("dltngus4860", "5150156q");
        }
    }

    private void Login(string ID, string PW)
    {
        var request = new LoginWithPlayFabRequest() { Username = ID, Password = PW};
        PopUp.I.OpenPopUp("로그인 중입니다");
        PlayFabClientAPI.LoginWithPlayFab(request,
            (result) =>
            {
                PopUp.I.ActiveCG(false);
                // SceneManager.LoadScene("1.Lobby");
                GameManager.I.playFabEntity = result.EntityToken;
                GameManager.I.ID = result.PlayFabId;
            },
            (error) =>
            {
                PopUp.I.OpenPopUp($"{error.ErrorMessage}\n{(int)error.Error}", () =>
                {
                    PopUp.I.ActiveCG(false);
                }, "확인");
            });
    }
    #endif

    [Button]
    public void Test133()
    {
        var dic = new Dictionary<string, float>()
        {
            { "A", 3 },
            { "B", 4 },
            { "C", 5 },
            { "D", 6 },
            { "E", 7 },
        };

        var str = JsonConvert.SerializeObject(dic);
        Debug.Log(str);
        var asd = JsonConvert.DeserializeObject<Dictionary<string, int>>(str);
        foreach (var VARIABLE in asd)
        {
            Debug.Log(VARIABLE);
        }
    }

    [Button]
    public void Test1()
    {
        var dic = new Dictionary<string, int>()
        {
            {"A", 3},
            {"B", 4},
            {"C", 5},
            {"D", 6},
            {"E", 7},
        };
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    { "Ancestor", JsonUtility.ToJson(dic) },
                },
                Permission = UserDataPermission.Private
            },
            result => Debug.Log("Successfully updated user data"),
            error =>
            {
                Debug.Log("Got error setting user data Ancestor to Arthur");
                Debug.Log(error.GenerateErrorReport());
            });
        // PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        //     {
        //         Keys = new List<string>()
        //         {
        //             "lqwdlq"
        //         },
        //         PlayFabId = ""
        //     }, 
        //     result => Debug.Log("Successfully updated user data"),
        //     error =>
        //     {
        //         Debug.Log("Got error setting user data Ancestor to Arthur");
        //         Debug.Log(error.GenerateErrorReport());
        //     });
        
    }
}
