using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CreateRoomPanel : MonoBehaviour, ISetInspector
{
    [SerializeField] private CanvasGroup cg;

    [SerializeField, FoldoutGroup("UIs")] private Toggle singleToggle;
    [SerializeField, FoldoutGroup("UIs")] private Toggle multiToggle;
    [SerializeField, FoldoutGroup("UIs")] private TMP_InputField roomNameIF;
    
    [SerializeField, FoldoutGroup("UIs")] private Toggle passwordToggle;
    [SerializeField, FoldoutGroup("UIs")] private TMP_InputField passwordIF;
    
    [SerializeField, FoldoutGroup("UIs")] private Button createBtn;
    [SerializeField, FoldoutGroup("UIs")] private Button cancelBtn;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.parent.GetAllChild();
        cg = gameObject.GetComponent<CanvasGroup>();
        singleToggle = childs.First(tr => tr.name == "Single Toggle").GetComponent<Toggle>();
        multiToggle = childs.First(tr => tr.name == "Multi Toggle").GetComponent<Toggle>();
        roomNameIF = childs.First(tr => tr.name == "RoomName IF").GetComponent<TMP_InputField>();
        passwordToggle = childs.First(tr => tr.name == "Password Toggle").GetComponent<Toggle>();
        passwordIF  = childs.First(tr => tr.name == "Password IF").GetComponent<TMP_InputField>();
        createBtn = childs.First(tr => tr.name == "Create Btn").GetComponent<Button>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
    }

    private void Start()
    {
        #region SetButtons
        singleToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = false;
            roomNameIF.text = string.Empty;
            passwordToggle.isOn = false;
            passwordToggle.interactable = false;
        });
        multiToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = true;
            passwordToggle.interactable = true;
        });
        passwordToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                passwordIF.text = string.Empty;
            passwordIF.interactable = isOn;
        });
        
        createBtn.onClick.AddListener(() =>
        {
            if (passwordToggle.isOn && passwordIF.text.IsNullOrEmpty())
            {
                PopUp.I.OpenPopUp(
                    "비밀번호 입력",
                    () => PopUp.I.ActiveCG(false),
                    "닫기");
                return;
            }
            
            App.I.HostGame(
                singleToggle.isOn ? GameMode.Single : GameMode.Host,
                passwordIF.text.IsNullOrEmpty() ? int.MaxValue : int.Parse(passwordIF.text),
                () =>
                {
                    cg.ActiveCG(false);
                });
        });
        
        cancelBtn.onClick.AddListener(() =>
        {
            cg.ActiveCG(false);
        });
        #endregion
    }
}
