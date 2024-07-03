using System.Text;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestSO questSO;

    public TextMeshProUGUI nowQuestText;
    private StringBuilder sb = new StringBuilder();

    public void Init()
    {

    }

    public void UpdateQuest()
    {
        sb.Append(questSO.quests[0]);
        nowQuestText.text = sb.ToString();
    }

    public void FinishQuest()
    {

    }

}