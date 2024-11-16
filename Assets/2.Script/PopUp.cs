using Fusion;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, ISetInspector
{
    [SerializeField] private TMP_Text msg;
    [SerializeField] private CanvasGroup cg;
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
        SetBtn(btn1);
        SetBtn(btn2);
        SetBtn(btn3);
        if (result.Ok)
            ActiveCG(false);
        else
        {
            msg.text = $"에러 {result.ErrorMessage}";
            SetBtn(btn1, () =>
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
        this.msg.text = msg;
        SetBtn(btn1, buttonAction1, buttonText1, buttonColor1);
        SetBtn(btn2, buttonAction2, buttonText2, buttonColor2);
        SetBtn(btn3, buttonAction3, buttonText3, buttonColor3);
    }
    
    private void SetBtn(Button button, UnityAction buttonAction = null, string buttonText = null, Color buttonColor = new())
    {
        button.gameObject.SetActive(false == buttonAction.IsUnityNull());
        if (false == buttonAction.IsUnityNull())
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(buttonAction);
            text1.text = buttonText;
            // button.targetGraphic.color = buttonColor;
        }
    }
}
