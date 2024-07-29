using System.Text;
using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestSO questSO;
    public int CurrentQuest = -1;
    public TextMeshProUGUI nowQuestText; // ���� ������ UIDialogue�� ��ü
    private StringBuilder sb = new StringBuilder();

    public void UpdateQuest()
    {

        CurrentQuest++;
        sb.Append(questSO.quests[CurrentQuest]);
        nowQuestText.text = "�� ��: " + sb.ToString();
    }   
}