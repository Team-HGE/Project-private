using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Script", order = 0)]
public class ScriptSO : ScriptableObject
{
    public Sprite[] images;
    public string[] speakers;
    [TextArea] public string[] bodyTexts;
    public AudioClip[] audioClips;
}


