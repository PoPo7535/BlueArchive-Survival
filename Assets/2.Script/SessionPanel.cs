using UnityEngine;
using UnityEngine.UI;

public class SessionPanel : MonoBehaviour
{
    public Button btn;
    // Update is called once per frame
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            App.I.StartGame();
        });
    }
}
