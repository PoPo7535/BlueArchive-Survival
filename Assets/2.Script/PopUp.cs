using System;
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

    public static PopUp I;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        I = this;
    }

    public void ShowHideCG(bool isShow)
    {
        cg.ShowHideCG(isShow);
    }
    
    public void OpenPopUp(string msg,
        UnityAction buttonAction1, string buttonText1, Color buttonColor1 = new(),
        UnityAction buttonAction2 = null, string buttonText2 = null, Color buttonColor2 = new(),
        UnityAction buttonAction3 = null, string buttonText3 = null, Color buttonColor3 = new())
    {
        this.msg.text = msg;
        btn1.onClick.AddListener(buttonAction1);
        text1.text = buttonText1;
        // btn1.targetGraphic.color = buttonColor1;
        
        btn2.gameObject.SetActive(false == buttonAction2.IsUnityNull());
        if (false == buttonAction2.IsUnityNull())
        {
            btn2.onClick.AddListener(buttonAction2);
            text2.text = buttonText2;
            // btn2.targetGraphic.color = buttonColor2;
        }
        btn3.gameObject.SetActive(false == buttonAction3.IsUnityNull());
        if (false == buttonAction3.IsUnityNull())
        {
            btn3.onClick.AddListener(buttonAction3);
            text3.text = buttonText3;
            // btn3.targetGraphic.color = buttonColor3;
        }

    }
}
