using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField idIF;
    [SerializeField] private TMP_InputField pwIF;
    [SerializeField] private Toggle idSaveToggle;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button registerBtn;
    private readonly PlayFabLogin playFabLogin = new();
    
    [Button,GUIColor(0, 1, 0)]
    private void SetInspector()
    {
        var childs = transform.GetAllChild();
        idIF = childs.First(tr => tr.name == "ID InputField").GetComponent<TMP_InputField>();
        pwIF = childs.First(tr => tr.name == "PW InputField").GetComponent<TMP_InputField>();
        idSaveToggle = childs.First(tr => tr.name == "IDSave Toggle").GetComponent<Toggle>();
        loginBtn  = childs.First(tr => tr.name == "Login Button").GetComponent<Button>();
        registerBtn = childs.First(tr => tr.name == "Register Button").GetComponent<Button>();
    }


    private void Start()
    {
        idIF.onValueChanged.AddListener((str) =>
        {
            loginBtn.interactable = idIF.text.Length >= 6 && pwIF.text.Length >= 6;
        });
        loginBtn.onClick.AddListener(() =>
        {
            playFabLogin.Login(idIF.text, pwIF.text);
        });
    }
}
