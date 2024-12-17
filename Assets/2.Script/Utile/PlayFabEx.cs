using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayFabEx
{

    private static Dictionary<PlayerStatus, float> GetStateData()
    {
        return new Dictionary<PlayerStatus, float>()
        {
            { PlayerStatus.Attack, 1F },
            { PlayerStatus.Speed, 1F },
        };
    }

    public enum PlayerStatus
    {
        Attack,
        Speed
    }

    public static void InitStateDate(Action<UpdateUserDataResult> result = null, Action<PlayFabError> error = null)
    {
        Debug.Log(JsonConvert.SerializeObject(GetStateData()));
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() { { nameof(PlayerStatus), JsonConvert.SerializeObject(GetStateData()) } }
            },
            _result => result?.Invoke(_result),
            _error => error?.Invoke(_error));
    }
    public static void UpdateStateData(Action<UpdateUserDataResult> result, Action<PlayFabError> error, Dictionary<PlayerStatus, float> data)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() { { nameof(PlayerStatus), JsonConvert.SerializeObject(data) } }
            },
            _result => result?.Invoke(_result),
            _error => error?.Invoke(_error));
    }
    public static void GetStateData(Action<GetUserDataResult> result, Action<PlayFabError> error, Action<Dictionary<PlayerStatus, float>> data)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = new List<string>() { nameof(PlayerStatus) },
                PlayFabId = GameManager.I.ID
            },
            _result =>
            {
                result?.Invoke(_result);
                var getState = JsonConvert.DeserializeObject<Dictionary<PlayerStatus, float>>(_result.Data[nameof(PlayerStatus)].Value);
                var newState = GetStateData();
                foreach (var value in getState)
                {
                    if (newState.ContainsKey(value.Key))
                        newState[value.Key] = value.Value;
                    else
                        Debug.LogWarning("유저 데이터 오버로딩 오류");
                }
                data?.Invoke(newState);
            },
            _error => error?.Invoke(_error));
        
    }
}
