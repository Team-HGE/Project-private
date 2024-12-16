using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Script", order = 0)]
public class ScriptSO : ScriptableObject
{
    [Header("Image")]
    public Sprite[] portraits;
    public Sprite[] standings;
    public Sprite[] BackGrounds;

    [Header("Text")]
    public string[] speakers;
    [TextArea] public string[] bodyTexts;

    [Header("Audio")]
    public AudioClip[] audioClips;
}


