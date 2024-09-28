using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayFabLogin
{

    public static void Login(string username, string password, Action resultAction = null, Action errorAction = null)
    {
        var request = new LoginWithPlayFabRequest() { Username = username, Password = password };
        PlayFabClientAPI.LoginWithPlayFab(request,
            (result) =>
            {
                resultAction?.Invoke();
            },
            (error) =>
            {
                errorAction?.Invoke();
            });

    }

    public static void Register(string displayName, string username, string password, Action resultAction = null,
        Action errorAction = null)
    {
        var request = new RegisterPlayFabUserRequest()
        {
            DisplayName = displayName, Username = username, Password = password, RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request,
            (result) =>
            {
                resultAction?.Invoke();
            },
            (error) =>
            {
                errorAction?.Invoke();
                Debug.Log(error.Error);
            });
    }

}
