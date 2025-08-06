using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Serial = UnityEngine.SerializeField;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;
public class LobbyRightWidget : MonoBehaviour, ISetInspector
{
    [Serial, Read] private Button sessionBtn;
    [Serial, Read] private CanvasGroup sessionPanel;
    [Serial, Read] private Button statusBtn;
    [Serial, Read] private CanvasGroup statusPanel;

    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.parent.GetAllChild();
        sessionBtn = childs.First(tr => tr.name == "Session Button").GetComponent<Button>();
        statusBtn = childs.First(tr => tr.name == "Status Button").GetComponent<Button>();
        sessionPanel = childs.First(tr => tr.name == "SessionPanel").GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        sessionBtn.onClick.AddListener(() =>
        {
            sessionPanel.ActiveCG(true);
            BG.GetBG(sessionPanel.transform, () =>
            {
                sessionPanel.ActiveCG(false);
            });
        });
        statusBtn.onClick.AddListener(() =>
        {
            // statusPanel.ActiveCG(true);
        });
    }

}
