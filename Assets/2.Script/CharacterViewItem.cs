using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Serial = UnityEngine.SerializeField;
using Read = Sirenix.OdinInspector.ReadOnlyAttribute;
using Fold = Sirenix.OdinInspector.FoldoutGroupAttribute;

public class CharacterViewItem : MonoBehaviour, ISetInspector
{
    [Serial, Read] public GameObject character;
    [Serial, Read] private new GameObject camera;
    private readonly Dictionary<PlayableChar, GameObject> dic = new();
    [Read] private PlayableChar current = PlayableChar.None;

    public void SetChar(PlayableChar ch)
    {
        if (current == ch)
            return;
        if (current != PlayableChar.None)
            dic[current].gameObject.SetActive(false);
        current = ch;
        if (dic.TryGetValue(ch, out var value))
        {
            value.gameObject.SetActive(true);
            return;
        }

        var playerChar = Instantiate(GameManager.I.playableChar[ch], Vector3.zero, Quaternion.identity, character.transform);
        playerChar.transform.localPosition = Vector3.zero;
        playerChar.transform.localScale = Vector3.one * 900;
        dic.Add(ch, playerChar);
    }

    public void Clear()
    {
        if(current == PlayableChar.None)
            return;
        dic[current].gameObject.SetActive(false);
    }

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        character = childs.First(tr => tr.name == "Character").gameObject;
        camera = childs.First(tr => tr.name == "Camera").gameObject;
    }

}
