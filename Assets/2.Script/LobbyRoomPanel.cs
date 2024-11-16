using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomPanel : MonoBehaviour, ISetInspector
{
    
    // [SerializeField] private Button slots;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Button readyBtn;
    [SerializeField] private Button cancelBtn;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        cg = GetComponent<CanvasGroup>();
        readyBtn = childs.First(tr => tr.name == "Ready Btn").GetComponent<Button>();
        cancelBtn = childs.First(tr => tr.name == "Cancel Btn").GetComponent<Button>();
    }
    private void Start()
    {
        cancelBtn.onClick.AddListener((() =>
        {
            cg.ActiveCG(false);
            App.I.runner.Shutdown(false);
        }));
    }

    private void Update()
    {
        
    }
}
