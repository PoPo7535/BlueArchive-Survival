using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SessionPanel : MonoBehaviour, ISetInspector
{
    [SerializeField] private Button createRoomPanelBtn;
    [SerializeField] private CanvasGroup createRoomPanelCG;
    
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.parent.GetAllChild();
        createRoomPanelBtn = childs.First(tr => tr.name == "CrateRoom Btn").GetComponent<Button>();
        createRoomPanelCG = childs.First(tr => tr.name == "CreateRoomPanel").GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        createRoomPanelBtn.onClick.AddListener(() =>
        {
            createRoomPanelCG.ActiveCG(true);
        });
    }

}
