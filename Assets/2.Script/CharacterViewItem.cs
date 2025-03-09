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
    [Read] private PlayableChar previous = PlayableChar.None;

    public void SetChar(PlayableChar ch)
    {
        if (previous == ch)
            return;
        if (previous != PlayableChar.None)
            dic[previous].gameObject.SetActive(false);
        previous = ch;

        var playerChar = Instantiate(GameManager.I.playableChar[ch], Vector3.zero, Quaternion.identity, character.transform);
        playerChar.transform.localPosition = Vector3.zero;
        playerChar.transform.localScale = Vector3.one * 900;
        dic.Add(ch,playerChar);
    }

    [Button, GUIColor(0, 1, 0)]
    public void SetInspector()
    {
        var childs = transform.GetAllChild();
        character = childs.First(tr => tr.name == "Character").gameObject;
        camera = childs.First(tr => tr.name == "Camera").gameObject;
    }

}
