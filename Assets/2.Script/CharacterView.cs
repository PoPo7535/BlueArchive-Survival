using System.Linq;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharacterView : MonoBehaviour, ISetInspector
{
    public GameObject[] playableChar;
    [Read, Serial]public CharacterViewItem[] views;
    
    [Button,GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        views = new CharacterViewItem[3];
        views[0] = childs.First(tr => tr.name == "View (2)").GetComponent<CharacterViewItem>();
        views[1] = childs.First(tr => tr.name == "View (1)").GetComponent<CharacterViewItem>();
        views[2] = childs.First(tr => tr.name == "View (3)").GetComponent<CharacterViewItem>();
    }
    
    public void SetChar(PlayerRef playerRef, PlayableChar ch)
    {
        views[App.I.GetPlayerIndex(playerRef)].SetChar(ch);
    }
    
    public async void SetAllChar()
    {
        var players = App.I.GetPlayers();
        for (int i = 0; i < 3; ++i)
        {
            if (i < players.Count)
            {
                await App.I.ConnectingPlayer(players[i]);
                views[i].SetChar(App.I.GetPlayerInfo(players[i]).CharIndex);
            }
            else
                views[i].Clear();
        }
    }
    
    public void Clear(PlayerRef playerRef)
    {
         views[App.I.GetPlayerIndex(playerRef)].Clear();
    }

    [Button]
    public void Test(int number, PlayableChar ch)
    {
        if (Application.isPlaying)
            views[number].SetChar(ch);
    }

}
