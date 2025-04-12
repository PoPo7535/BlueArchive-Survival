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
    [Serial, Read, Fold("UIs")] private TMP_InputField passwordIF;
    [Serial, Read, Fold("UIs")] private Button createBtn;                                                                                    
    [Serial, Read, Fold("UIs")] private Button cancelBtn;
    
    
    [Serial, Read, Fold("UIs")] private Image lookImg;
    [Serial, Read, Fold("UIs")] private TMP_Text lookText;
    [Serial, Fold("UIs")] private Sprite lookSpr;
    [Serial, Fold("UIs")] private Sprite openSpr;
           
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.parent.GetAllChild();
        cg = gameObject.GetComponent<CanvasGroup>();
        singleToggle = childs.First(tr => tr.name == "Single Toggle").GetComponent<Toggle>();
        multiToggle = childs.First(tr => tr.name == "Multi Toggle").GetComponent<Toggle>();
        roomNameIF = childs.First(tr => tr.name == "RoomName IF").GetComponent<TMP_InputField>();
        passwordIF  = childs.First(tr => tr.name == "Password IF").GetComponent<TMP_InputField>();
        createBtn = childs.First(tr => tr.name == "Create Btn").GetComponent<Button>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
        lookImg = childs.First(tr => tr.name == "Look").GetComponent<Image>();
        lookText = childs.First(tr => tr.name == "Look Text").GetComponent<TMP_Text>();
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
            roomNameIF.text = string.Empty;
            passwordIF.interactable = false;
            lookImg.gameObject.SetActive(false);
        });
        multiToggle.onValueChanged.AddListener(isOn =>
        {
            if (false == isOn)
                return;
            roomNameIF.interactable = true;
            passwordIF.interactable = true;
            SetCreateBtn(string.Empty);
            lookImg.gameObject.SetActive(true);
        });

        
        createBtn.onClick.AddListener(() =>
        {

            App.I.HostGame(
                singleToggle.isOn ? GameMode.Single : GameMode.Host,
                roomNameIF.text, 
                passwordIF.text,
                () => { cg.ActiveCG(false); });
        });
        
        roomNameIF.onValueChanged.AddListener(SetCreateBtn);
        passwordIF.onValueChanged.AddListener(SetCreateBtn);
        
        cancelBtn.onClick.AddListener(Disable);

        void SetCreateBtn(string _)
        {
            if (passwordIF.text.IsNullOrEmpty())
            {
                lookImg.sprite = openSpr;
                lookText.text = "현재 공개 방입니다.";
            }
            else 
            {
                lookImg.sprite = lookSpr;
                lookText.text = "현재 비공개 방입니다.";
            }
            
            if (singleToggle.isOn)
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

    public void Disable()
    {
        cg.ActiveCG(false);
        roomNameIF.text = string.Empty;
        passwordIF.text = string.Empty;
        singleToggle.isOn = true;
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
