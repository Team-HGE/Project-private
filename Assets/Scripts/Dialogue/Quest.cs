using System.Text;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestSO questSO;
    public int CurrentQuest = -1;
    public TextMeshProUGUI nowQuestText;
    private StringBuilder sb = new StringBuilder();

    public void Init()
    {
        
    }

    public void UpdateQuest()
    {

        CurrentQuest++;
        sb.Append(questSO.quests[CurrentQuest]);
        nowQuestText.text = "«“ ¿œ: " + sb.ToString();
    }


    public void FinishQuest()
    {

    }
   
}