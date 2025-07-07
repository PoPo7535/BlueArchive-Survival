using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public partial class BattleSceneManager
{
    private bool _mouseButton0;

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.buttons.Set( NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        _mouseButton0 = false;
        
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        data.input = new Vector2(x, y);
        input.Set(data);
    }

    public void Update()
    {
        _mouseButton0 |= Input.GetMouseButton(0);
    }
    private void OnEnable()
    {
        App.I.Runner.AddCallbacks(this);
    }

    private void OnDisable()
    {
        App.I.Runner.RemoveCallbacks(this);
    }
}
public struct NetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON0 = 1;
    public NetworkButtons buttons;
    public Vector2 input;
}