using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickLogin : MonoBehaviour
{
    private bool isLogin = false;
    #if UNITY_EDITOR
    private void OnGUI()
    {
        if (isLogin)
            return;
        
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
                SceneManager.LoadScene("1.Lobby");
                GameManager.I.playFabEntity = result.EntityToken;
                GameManager.I.ID = result.PlayFabId;
                isLogin = true;
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
}
