using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalPlayer
{
    public static void ReSet()
    {
        _playerBase = null;
        _skillManager = null;
    }
    
    public static PlayerBase _playerBase;
    public static PlayerBase PlayerBase
    {
        get
        {
            if (null == _playerBase)
                _playerBase = App.I.GetPlayerInfo(App.I.Runner.LocalPlayer).PlayerObject;
            return _playerBase;
        }
    }
    
    private static PlayerSkillManager _skillManager;
    public static PlayerSkillManager SkillManager
    {
        get
        {
            if (null == _skillManager)
                _skillManager = PlayerBase.SkillManager;
            return _skillManager;
        }
    }
}
