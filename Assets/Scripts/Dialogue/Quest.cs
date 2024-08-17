using System.Runtime.CompilerServices;
using System;
using System.Text;
using TMPro;
using UnityEngine;

public class Quest : SingletonManager<Quest>
{
    public QuestSO questSO;
    public int CurrentQuestIndex;
    public TextMeshProUGUI nowQuestText; // 협업 끝나면 UIDialogue로 대체
    private StringBuilder sb = new StringBuilder();
    public AudioManager audioManager;
    public GameObject questCanvas;


    private void Start() 
    {
        audioManager = GetComponent<AudioManager>();
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
    public void TryNextQuest()
    {
        CurrentQuestIndex++;
        UpdateQuest();
        AudioManager.Instance.PlaySoundEffect(SoundEffect.Quest);
    }

}