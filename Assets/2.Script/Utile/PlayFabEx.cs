using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayFabEx
{

    #region Status
    private static Dictionary<StatusType, float> GetStateData()
    {
        return new Dictionary<StatusType, float>()
        {
            // { PlayerStatus.AttackSpeed, 1F },
            // { PlayerStatus.MoveSpeed, 1F },
        };
    }
    public enum StatusType
    {
        MaxHP,              // 최대 체력
        HPRegen,            // 체력재생
        Defense,            // 방어력
        MoveSpeed,          // 이동속도
        Damage,             // 데미지
        CriticalChance,     // 치명타 확률
        CriticalDamage,     // 치명타 데미지
        ProjectileCount,    // 투사체 개수
        Duration,           // 지속시간
        CoolDown,           // 재사용 대기시간
        PickupRange,        // 획득범위
        AreaSize,           // 범위
        EXPGain,            // 경험치 획득
        GoldGain,           // 골드 획득
    }
    


    public static void InitStatusDate(Action<UpdateUserDataResult> result = null, Action<PlayFabError> error = null)
    {
        Debug.Log(JsonConvert.SerializeObject(GetStateData()));
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() { { nameof(StatusType), JsonConvert.SerializeObject(GetStateData()) } }
            },
            _result => result?.Invoke(_result),
            _error => error?.Invoke(_error));
    }
    public static void UpdateStatusData(Action<UpdateUserDataResult> result, Action<PlayFabError> error, Dictionary<StatusType, float> data)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() { { nameof(StatusType), JsonConvert.SerializeObject(data) } }
            },
            _result => result?.Invoke(_result),
            _error => error?.Invoke(_error));
    }
    // https://docs.google.com/spreadsheets/d/1sJYV-jYntE5F164HziNjNWVZhTuSyU3a5v8vflrA89Y/edit?usp=drive_link
    public static void GetStatusData(Action<GetUserDataResult> result, Action<PlayFabError> error, Action<Dictionary<StatusType, float>> data)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                Keys = new List<string>() { nameof(StatusType) },
                PlayFabId = GameManager.I.ID
            },
            _result =>
            {
                result?.Invoke(_result);
                var getState = JsonConvert.DeserializeObject<Dictionary<StatusType, float>>(_result.Data[nameof(StatusType)].Value);
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
    #endregion

}
