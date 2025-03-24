using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class RoomCanvas : MonoBehaviour, ISetInspector
{
    [Read, Serial] private Button cancelBtn;
    [Read, Serial] private Button charSelectBtn;
    [Read, Serial] private CharSelectPanel charSelectPanel;

    [Button, GUIColor(0,1,0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
        charSelectBtn = childs.First(tr => tr.name == "CharSelect Btn").GetComponent<Button>();
        charSelectPanel = childs.First(tr => tr.name == "CharSelectPanel").GetComponent<CharSelectPanel>();
    }

    public void Start()
    {
        cancelBtn.onClick.AddListener(Call);
        async void Call()
        {
            await App.I.runner.Shutdown(false);
            SceneManager.LoadScene("1.Lobby");
        }
        
        charSelectBtn.onClick.AddListener( () =>
        {
            charSelectPanel.Open();
        });
    }
}
