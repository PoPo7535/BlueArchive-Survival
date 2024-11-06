using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour, ISetInspector
{
    [SerializeField, FoldoutGroup("LoginBox")] private CanvasGroup loginGroup;
    [SerializeField, FoldoutGroup("LoginBox")] private TMP_InputField loginIdIF;
    [SerializeField, FoldoutGroup("LoginBox")] private TMP_InputField loginPwIF;
    [SerializeField, FoldoutGroup("LoginBox")] private Toggle idSaveToggle;
    [SerializeField, FoldoutGroup("LoginBox")] private Button loginBtn;
    [SerializeField, FoldoutGroup("LoginBox")] private Button registerOpenBtn;
    
    [SerializeField, FoldoutGroup("RegisterPanel")] private CanvasGroup registerGroup;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField nickNameIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_Text nickNameWarningText;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerIdIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_Text registerIdWarningText;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerPwIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_Text registerPwWarningText;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_InputField registerPwCheckIF;
    [SerializeField, FoldoutGroup("RegisterPanel")] private TMP_Text registerPwCheckPwWarningText;
    [SerializeField, FoldoutGroup("RegisterPanel")] private Button registerBtn;
    [SerializeField, FoldoutGroup("RegisterPanel")] private Button registerCancelBtn;

    private const string IDSave = "IDSave";
    private const string ID = "ID";
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        loginGroup = childs.First(tr => tr.name == "LoginBox").GetComponent<CanvasGroup>();
        loginIdIF = childs.First(tr => tr.name == "LoginID IF").GetComponent<TMP_InputField>();
        loginPwIF = childs.First(tr => tr.name == "LoginPW IF").GetComponent<TMP_InputField>();
        idSaveToggle = childs.First(tr => tr.name == "IDSave Tg").GetComponent<Toggle>();
        loginBtn  = childs.First(tr => tr.name == "Login Btn").GetComponent<Button>();
        registerOpenBtn = childs.First(tr => tr.name == "RegisterOpen Btn").GetComponent<Button>();

        registerGroup = childs.First(tr => tr.name == "RegisterBox").GetComponent<CanvasGroup>();
        nickNameIF = childs.First(tr => tr.name == "NickName IF").GetComponent<TMP_InputField>();
        nickNameWarningText= nickNameIF.transform.GetChild(1).GetComponent<TMP_Text>();
        registerIdIF = childs.First(tr => tr.name == "RegisterID IF").GetComponent<TMP_InputField>();
        registerIdWarningText= registerIdIF.transform.GetChild(1).GetComponent<TMP_Text>();
        registerPwIF = childs.First(tr => tr.name == "RegisterPW IF").GetComponent<TMP_InputField>();
        registerPwWarningText= registerPwIF.transform.GetChild(1).GetComponent<TMP_Text>();
        registerPwCheckIF = childs.First(tr => tr.name == "PWCheck IF").GetComponent<TMP_InputField>();
        registerPwCheckPwWarningText= registerPwCheckIF.transform.GetChild(1).GetComponent<TMP_Text>();
        registerBtn = childs.First(tr => tr.name == "Register Btn").GetComponent<Button>();
        registerCancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
    }


    private void Start()
    {
        Debug.Log(GameManager.I.gameObject);
        Debug.Log(PopUp.I.gameObject);

        SetLoginBox();
        SetRegisterBox();
        SetWarningText();
        idSaveToggle.isOn = PlayerPrefs.GetInt(IDSave) == 1;
        if (idSaveToggle.isOn) 
            loginIdIF.text = PlayerPrefs.GetString(ID);
    }

    private void SetLoginBox()
    {
        loginIdIF.onValueChanged.AddListener(SetLoginBtn);
        loginPwIF.onValueChanged.AddListener(SetLoginBtn);
        loginBtn.onClick.AddListener(() =>
        {
            var request = new LoginWithPlayFabRequest() { Username = loginIdIF.text, Password = loginPwIF.text };
            PlayFabClientAPI.LoginWithPlayFab(request,
                (result) =>
                {
                    SceneManager.LoadScene("1.Lobby");
                    PlayerPrefs.SetInt(IDSave, idSaveToggle.isOn ? 1 : 0);
                    PlayerPrefs.SetString(ID, idSaveToggle.isOn ? loginIdIF.text : string.Empty);
                },
                (error) =>
                {
                    PopUp.I.ShowHideCG(true);
                    PopUp.I.OpenPopUp($"{error.ErrorMessage}\n{(int)error.Error}", () =>
                    {
                        PopUp.I.ShowHideCG(false);
                    }, "확인");
                });
        });
        registerOpenBtn.onClick.AddListener(() =>
        {
            loginGroup.ShowHideCG(false);
            registerGroup.ShowHideCG(true);
            loginIdIF.text = string.Empty;
            loginPwIF.text = string.Empty;
        });

        void SetLoginBtn(string str)
        {
            loginBtn.interactable = Define.accountMinLength < loginIdIF.text.Length &&
                                    Define.accountMinLength < loginPwIF.text.Length;
        }
    }

    private void SetRegisterBox()
    {
        registerCancelBtn.onClick.AddListener(() =>
        {
            loginGroup.ShowHideCG(true);
            registerGroup.ShowHideCG(false);
            registerIdIF.text = string.Empty;
            registerPwIF.text = string.Empty;
            registerPwCheckIF.text = string.Empty;
            nickNameIF.text = string.Empty;
        });
        nickNameIF.onValidateInput = (_, _, addedChar) => IsAllowedCharacter(addedChar) ? addedChar : '\0';
        
        SetRegisterIF(nickNameIF, nickNameWarningText, Define.nickNameMinLength);
        SetRegisterIF(registerIdIF, registerIdWarningText, Define.accountMinLength);
        SetRegisterIF(registerPwIF, registerPwWarningText, Define.accountMinLength);
        SetRegisterIF(registerPwIF, registerPwCheckPwWarningText, Define.accountMinLength, true);
        SetRegisterIF(registerPwCheckIF, registerPwCheckPwWarningText, Define.accountMinLength, true);

        registerBtn.onClick.AddListener(() =>
        {
            var request = new RegisterPlayFabUserRequest()
            {
                DisplayName = nickNameIF.text, Username = registerIdIF.text, Password = registerPwIF.text, RequireBothUsernameAndEmail = false
            };
            Debug.Log($"{nickNameIF.text} {loginIdIF.text} {loginPwIF.text}");
            PlayFabClientAPI.RegisterPlayFabUser(request,
                (result) =>
                {
                    PopUp.I.ShowHideCG(true);
                    PopUp.I.OpenPopUp($"회원가입에 성공하였습니다.", () =>
                    {
                        PopUp.I.ShowHideCG(false);
                        registerCancelBtn.onClick.Invoke();
                    }, "확인");
                },
                (error) =>
                {
                    PopUp.I.ShowHideCG(true);
                    PopUp.I.OpenPopUp($"{error.ErrorMessage}\n{(int)error.Error}", 
                        () => 
                        { 
                            PopUp.I.ShowHideCG(false); 
                        }, "확인");
                });
        });

        void SetRegisterIF(TMP_InputField inputField, TMP_Text warningText, int min, bool pwCheck = false)
        {
            inputField.onValueChanged.AddListener(str=>
            {
                registerBtn.interactable = Define.nickNameMinLength <= nickNameIF.text.Length && 
                                                       Define.accountMinLength <= registerIdIF.text.Length &&
                                                       Define.accountMinLength <= registerPwIF.text.Length &&
                                                       Define.accountMinLength <= registerPwCheckIF.text.Length &&
                                                       registerPwIF.text == registerPwCheckIF.text;
                if (false == pwCheck)
                    warningText.gameObject.SetActive(inputField.text.Length < min);
                else
                    warningText.gameObject.SetActive(registerPwIF.text != registerPwCheckIF.text);
            });
        }
    }

    private void SetWarningText()
    {
        nickNameWarningText.text = $"닉네임은 최소 {Define.nickNameMinLength}글자 이상 이여야 합니다.";
        registerIdWarningText.text = $"아이디는 최소 {Define.accountMinLength}글자 이상 이여야 합니다.";
        registerPwWarningText.text = $"비밀번호는 최소 {Define.accountMinLength}글자 이상 이여야 합니다.";
        registerPwCheckPwWarningText.text = $"비밀번호가 동일하지 않습니다.";
    }
    private bool IsAllowedCharacter(char c)
    {
        return c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '가' and <= '힣' or >= '0' and <= '9';
    }

}
