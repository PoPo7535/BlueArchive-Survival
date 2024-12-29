using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, ISetInspector
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private TMP_Text msg;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField, FoldoutGroup("Button1")] private Button btn1;
    [SerializeField, FoldoutGroup("Button1")] private TMP_Text text1;
    [SerializeField, FoldoutGroup("Button2")] private Button btn2;
    [SerializeField, FoldoutGroup("Button2")] private TMP_Text text2;
    [SerializeField, FoldoutGroup("Button3")] private Button btn3;
    [SerializeField, FoldoutGroup("Button3")] private TMP_Text text3;
    
    public static PopUp I;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var list = transform.GetAllChild();
        cg = gameObject.GetComponent<CanvasGroup>();
        msg = list.Find(o => o.name == "Msg").GetComponent<TMP_Text>();
        inputField = list.Find(o => o.name == "IF").GetComponent<TMP_InputField>();
        btn1 = list.Find(o => o.name == "Btn1").GetComponent<Button>();
        text1 = list.Find(o => o.name == "Text1").GetComponent<TMP_Text>();
        btn2 = list.Find(o => o.name == "Btn2").GetComponent<Button>();
        text2 = list.Find(o => o.name == "Text2").GetComponent<TMP_Text>();
        btn3 = list.Find(o => o.name == "Btn3").GetComponent<Button>();
        text3 = list.Find(o => o.name == "Text3").GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        I = this;
    }

    public void ActiveCG(bool active)
    {
        cg.ActiveCG(active);
    }
    public void OpenPopUp(StartGameResult result)
    {
        SetBtn(btn1, text1);
        SetBtn(btn2, text2);
        SetBtn(btn3, text3);
        if (result.Ok)
            ActiveCG(false);
        else
        {
            msg.text = $"에러 {result.ErrorMessage}";
            SetBtn(btn1, text1, () =>
            {
                ActiveCG(false);
            });
        }
    }
    public void OpenPopUp(string msg,
        UnityAction buttonAction1 = null, string buttonText1 = null, Color buttonColor1 = new(),
        UnityAction buttonAction2 = null, string buttonText2 = null, Color buttonColor2 = new(),
        UnityAction buttonAction3 = null, string buttonText3 = null, Color buttonColor3 = new())
    {
        ActiveCG(true);
        inputField.gameObject.SetActive(false);
        this.msg.text = msg;

        SetBtn(btn1, text1, buttonAction1, buttonText1, buttonColor1);
        SetBtn(btn2, text2, buttonAction2, buttonText2, buttonColor2);
        SetBtn(btn3, text3, buttonAction3, buttonText3, buttonColor3);
    }

    public void OpenInputPopUp(string msg,
        UnityAction<string> buttonAction1 = null, string buttonText1 = null, Color buttonColor1 = new(),
        UnityAction<string> buttonAction2 = null, string buttonText2 = null, Color buttonColor2 = new(),
        UnityAction<string> buttonAction3 = null, string buttonText3 = null, Color buttonColor3 = new())
    {
        ActiveCG(true);
        inputField.gameObject.SetActive(true);
        this.msg.text = msg;
        
        SetInputBtn(btn1, text1, buttonAction1, buttonText1, buttonColor1);
        SetInputBtn(btn2, text2, buttonAction2, buttonText2, buttonColor2);
        SetInputBtn(btn3, text3, buttonAction3, buttonText3, buttonColor3);
    }
    
    private void SetBtn(Button button, TMP_Text text, UnityAction buttonAction = null, string buttonText = null, Color buttonColor = new())
    {
        button.gameObject.SetActive(false == buttonAction.IsUnityNull());
        if (false == buttonAction.IsUnityNull())
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(buttonAction);
            text.text = buttonText;
            // button.targetGraphic.color = buttonColor; 
        }
    }
    private void SetInputBtn(Button button, TMP_Text text, UnityAction<string> buttonAction = null, string buttonText = null, Color buttonColor = new())
    {
        button.gameObject.SetActive(false == buttonAction.IsUnityNull());
        if (false == buttonAction.IsUnityNull())
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>buttonAction?.Invoke(inputField.text));
            text.text = buttonText;
            // button.targetGraphic.color = buttonColor;
        }
    }
}
