using System.Text;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Quest : SingletonManager<Quest>
{
    public QuestSO questSO;
    public int CurrentQuestIndex;
    public TextMeshProUGUI nowQuestText; // 협업 끝나면 UIDialogue로 대체
    private StringBuilder sb = new StringBuilder();
    public AudioManager audioManager;
    public GameObject questCanvas;

    public Transform questCanvas_Transform;

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
        Sequence sequence = DOTween.Sequence();

        sequence.Append(questCanvas_Transform.DOLocalMoveX(-1500 , 1.5f)).SetEase(Ease.InBack);
        sequence.AppendInterval(0.5f);
        sequence.Append(questCanvas_Transform.DOLocalMoveX(-935, 1.5f)).SetEase(Ease.OutBack);

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