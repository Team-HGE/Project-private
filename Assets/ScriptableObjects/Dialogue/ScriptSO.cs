using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Script", order = 0)]
public class ScriptSO : ScriptableObject
{
    public Sprite[] portraits;
    public Sprite[] BackGrounds;
    public string[] speakers;
    [TextArea] public string[] bodyTexts;
    public AudioClip[] audioClips;
}


