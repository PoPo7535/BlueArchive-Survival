using System;
using EnhancedScrollerDemos.SuperSimpleDemo;
using EnhancedUI.EnhancedScroller;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionScrollCell : EnhancedScrollerCellView
{
    public TMP_Text someTextText;
    public Button btn;

    public void SetData(SessionInfo data)
    {
        someTextText.text = data.Name;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            var password = App.Property.Password.ToString();
            if (data.Properties.TryGetValue(password, out var property))
            {
                var result = (int)property.PropertyValue;
                PopUp.I.OpenInputPopUp("비밀번호 입력",
                    (input) =>
                    {
                        if (int.Parse(input) == result)
                            App.I.JoinGame(data.Name);
                        else
                        {
                            PopUp.I.OpenPopUp("비밀번호 오류",
                                () => { PopUp.I.ActiveCg(false); },
                                "확인");
                        }
                    }, "확인");
            }
            else
            {
                App.I.JoinGame(data.Name);
            }
        });
    }
}
