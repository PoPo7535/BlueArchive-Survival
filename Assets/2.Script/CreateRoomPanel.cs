using System;
using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CreateRoomPanel : MonoBehaviour, ISetInspector
{
    [Serial] private CanvasGroup cg;
           
    [Serial, Read, Fold("UIs")] private Toggle singleToggle;
    [Serial, Read, Fold("UIs")] private Toggle multiToggle;
    [Serial, Read, Fold("UIs")] private TMP_InputField roomNameIF;
           
    [Serial, Read, Fold("UIs")] private Toggle passwordToggle;
    [Serial, Read, Fold("UIs")] private TMP_InputField passwordIF;
           
    [Serial, Read, Fold("UIs")] private Button createBtn;                                                                                    
    [Serial, Read, Fold("UIs")] private Button cancelBtn;
           
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
            createBtn.interactable = true;
            roomNameIF.interactable = false;
            passwordToggle.isOn = false;
            passwordToggle.interactable = false;
            roomNameIF.text = string.Empty;
        });
        multiToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = true;
            passwordToggle.interactable = true;
            SetCreateBtn(string.Empty);
        });
        passwordToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
            {
                passwordIF.text = string.Empty;
                SetCreateBtn(string.Empty);
            }
            else
                createBtn.interactable = false;
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
                roomNameIF.text, 
                passwordIF.text,
                () => { cg.ActiveCG(false); });
        });
        
        roomNameIF.onValueChanged.AddListener(SetCreateBtn);
        passwordIF.onValueChanged.AddListener(SetCreateBtn);
        
        cancelBtn.onClick.AddListener(() => { cg.ActiveCG(false); });

        void SetCreateBtn(string _)
        {
            if (singleToggle.isOn)
            {
                createBtn.interactable = true;
                return;
            }

            if (passwordToggle.isOn &&
                false == roomNameIF.text.IsNullOrEmpty() ||
                false == passwordIF.text.IsNullOrEmpty())
            {
                createBtn.interactable = true;
                return;
            }

            if (multiToggle.isOn &&
                false == roomNameIF.text.IsNullOrEmpty())
            {
                createBtn.interactable = true;
                return;
            }
            
            createBtn.interactable = false;
        }
        #endregion
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 200, 200), "Host"))
        {
            App.I.HostGame(
                GameMode.Host,
                "Host",
                passwordIF.text);
        }
    }
    #endif
}
