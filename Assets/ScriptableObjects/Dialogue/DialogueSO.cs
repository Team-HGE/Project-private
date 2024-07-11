using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Dialogue", order = 0)]
public class DialogueSO : ScriptableObject
{
    public Sprite[] images;
    public string[] speakers;
    [TextArea] public string[] bodyTexts;
    public AudioClip[] audioClip;
}


