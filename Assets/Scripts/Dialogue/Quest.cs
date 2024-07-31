using System.Text;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestSO questSO;
    public int CurrentQuest = -1;
    public TextMeshProUGUI nowQuestText; // 협업 끝나면 UIDialogue로 대체
    private StringBuilder sb = new StringBuilder();

    public void UpdateQuest()
    {

        CurrentQuest++;
        sb.Append(questSO.quests[CurrentQuest]);
        nowQuestText.text = "할 일: " + sb.ToString();
    }   
}