using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class LoginPanel : MonoBehaviour, ISetInspector
{
    [Serial, Read, Fold("LoginBox")] private CanvasGroup loginGroup;
    [Serial, Read, Fold("LoginBox")] private TMP_InputField loginIdIF;
    [Serial, Read, Fold("LoginBox")] private TMP_InputField loginPwIF;
    [Serial, Read, Fold("LoginBox")] private Toggle idSaveToggle;
    [Serial, Read, Fold("LoginBox")] private Button loginBtn;
    [Serial, Read, Fold("LoginBox")] private Button registerOpenBtn;
    
    [Serial, Read, Fold("RegisterPanel")] private CanvasGroup registerGroup;
    [Serial, Read, Fold("RegisterPanel")] private TMP_InputField nickNameIF;
    [Serial, Read, Fold("RegisterPanel")] private TMP_Text nickNameWarningText;
    [Serial, Read, Fold("RegisterPanel")] private TMP_InputField registerIdIF;
    [Serial, Read, Fold("RegisterPanel")] private TMP_Text registerIdWarningText;
    [Serial, Read, Fold("RegisterPanel")] private TMP_InputField registerPwIF;
    [Serial, Read, Fold("RegisterPanel")] private TMP_Text registerPwWarningText;
    [Serial, Read, Fold("RegisterPanel")] private TMP_InputField registerPwCheckIF;
    [Serial, Read, Fold("RegisterPanel")] private TMP_Text registerPwCheckPwWarningText;
    [Serial, Read, Fold("RegisterPanel")] private Button registerBtn;
    [Serial, Read, Fold("RegisterPanel")] private Button registerCancelBtn;

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
            PopUp.I.OpenPopUp("로그인 중입니다");
            PlayFabClientAPI.LoginWithPlayFab(request,
                (result) =>
                {
                    PopUp.I.ActiveCG(false);
                    SceneManager.LoadScene("1.Lobby");
                    PlayerPrefs.SetInt(IDSave, idSaveToggle.isOn ? 1 : 0);
                    PlayerPrefs.SetString(ID, idSaveToggle.isOn ? loginIdIF.text : string.Empty);
                    GameManager.I.playFabEntity = result.EntityToken;
                    GameManager.I.ID = result.PlayFabId;
                },
                (error) =>
                {
                    PopUp.I.OpenPopUp($"{error.ErrorMessage}\n{(int)error.Error}", () =>
                    {
                        PopUp.I.ActiveCG(false);
                    }, "확인");
                });
        });
        registerOpenBtn.onClick.AddListener(() =>
        {
            loginGroup.ActiveCG(false);
            registerGroup.ActiveCG(true);
            loginIdIF.text = string.Empty;
            loginPwIF.text = string.Empty;
        });

        void SetLoginBtn(string str)
        {
            loginBtn.interactable = Define.accountMinLength <= loginIdIF.text.Length &&
                                    Define.accountMinLength <= loginPwIF.text.Length;
        }
    }

    private void SetRegisterBox()
    {
        registerCancelBtn.onClick.AddListener(() =>
        {
            loginGroup.ActiveCG(true);
            registerGroup.ActiveCG(false);
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
                    Debug.Log(1);
                    PlayFabEx.InitStatusDate(_ =>
                    {
                        Debug.Log(2);
                    }, _ =>
                    {
                        Debug.Log(3);
                    });
                    
                    PopUp.I.OpenPopUp($"회원가입에 성공하였습니다.", () =>
                    {
                        PopUp.I.ActiveCG(false);
                        registerCancelBtn.onClick.Invoke();
                    }, "확인");
                    
                },
                (error) =>
                {
                    PopUp.I.OpenPopUp($"{error.ErrorMessage}\n{(int)error.Error}", 
                        () => 
                        { 
                            PopUp.I.ActiveCG(false); 
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

