using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField, FoldoutGroup("LoginBox")] private CanvasGroup loginGroup;
    [SerializeField, FoldoutGroup("LoginBox")] private TMP_InputField loginIdIF;
    [SerializeField, FoldoutGroup("LoginBox")] private TMP_InputField loginPwIF;
    [SerializeField, FoldoutGroup("LoginBox")] private Toggle idSaveToggle;
    [SerializeField, FoldoutGroup("LoginBox")] private Button loginBtn;
    [SerializeField, FoldoutGroup("LoginBox")] private Button registerOpenBtn;
    
    [SerializeField, FoldoutGroup("RegisterPanel")] private CanvasGroup registerGroup;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerIdIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerPwIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerPwCheckIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private Button registerBtn;
    [SerializeField, FoldoutGroup("RegisterPanel")] private Button registerCancelBtn;
    [Button,GUIColor(0, 1, 0)]
    private void SetInspector()
    {
        var childs = transform.GetAllChild();
        loginGroup = childs.First(tr => tr.name == "LoginBox").GetComponent<CanvasGroup>();
        loginIdIF = childs.First(tr => tr.name == "LoginID IF").GetComponent<TMP_InputField>();
        loginPwIF = childs.First(tr => tr.name == "LoginPW IF").GetComponent<TMP_InputField>();
        idSaveToggle = childs.First(tr => tr.name == "IDSave Tg").GetComponent<Toggle>();
        loginBtn  = childs.First(tr => tr.name == "Login Btn").GetComponent<Button>();
        registerOpenBtn = childs.First(tr => tr.name == "RegisterOpen Btn").GetComponent<Button>();

        registerGroup = childs.First(tr => tr.name == "RegisterBox").GetComponent<CanvasGroup>();
        registerIdIF = childs.First(tr => tr.name == "RegisterID IF").GetComponent<TMP_InputField>();
        registerPwIF = childs.First(tr => tr.name == "RegisterPW IF").GetComponent<TMP_InputField>();
        registerPwCheckIF = childs.First(tr => tr.name == "PWCheck IF").GetComponent<TMP_InputField>();
        registerBtn = childs.First(tr => tr.name == "Register Btn").GetComponent<Button>();
        registerCancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
    }


    private void Start()
    {
        SetLoginBox();
        SetRegisterBox();
    }

    private void SetLoginBox()
    {
        loginIdIF.onValueChanged.AddListener(SetLoginBtn);
        loginPwIF.onValueChanged.AddListener(SetLoginBtn);
        loginBtn.onClick.AddListener(() =>
        {
            PlayFabLogin.Login(loginIdIF.text, loginPwIF.text);
        });
        registerOpenBtn.onClick.AddListener(() =>
        {
            loginGroup.SetActive(false);
            registerGroup.SetActive(true);
            loginIdIF.text = string.Empty;
            loginPwIF.text = string.Empty;
        });

        void SetLoginBtn(string str)
        {
            loginBtn.interactable = loginIdIF.text.Length >= 6 && loginPwIF.text.Length >= 6;
        }
    }

    private void SetRegisterBox()
    {
        registerCancelBtn.onClick.AddListener(() =>
        {
            loginGroup.SetActive(true);
            registerGroup.SetActive(false);
            registerIdIF.text = string.Empty;
            registerPwIF.text = string.Empty;
            registerPwCheckIF.text = string.Empty;
        });
        registerIdIF.onValueChanged.AddListener(SetRegisterBtn);
        registerPwIF.onValueChanged.AddListener(SetRegisterBtn);
        registerPwCheckIF.onValueChanged.AddListener(SetRegisterBtn);
        registerBtn.onClick.AddListener(() =>
        {
            PlayFabLogin.Register("", loginIdIF.text, loginPwIF.text);
        });
        void SetRegisterBtn(string str)
        {
            registerBtn.interactable = registerIdIF.text.Length >= 6 && registerPwIF.text.Length >= 6 && registerPwCheckIF.text.Length >= 6;
        }
    }
}
