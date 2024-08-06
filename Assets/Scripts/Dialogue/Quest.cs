using System.Text;
using TMPro;
using UnityEngine;
using static AudioManager;

public class Quest : MonoBehaviour
{
    public QuestSO questSO;
    public int CurrentQuestIndex;
    public TextMeshProUGUI nowQuestText; // 협업 끝나면 UIDialogue로 대체
    private StringBuilder sb = new StringBuilder();
    public AudioManager audioManager;

    private void Start() 
    {
        Init();
    }

    public void Init()
    {
        CurrentQuestIndex = 0;
        UpdateQuest();
    }

    public void UpdateQuest()
    {
        sb.Clear();
        sb.Append(questSO.quests[CurrentQuestIndex]);
        nowQuestText.text = "" + sb.ToString();

    }
    public void NextQuest(int newQuestIndex)
    {
        CurrentQuestIndex = newQuestIndex;
        UpdateQuest();
        AudioManager.Instance.PlaySoundEffect(SoundEffect.Quest);
    }
}