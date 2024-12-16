using UnityEngine;

public class Npc_JOE_Trigger : MonoBehaviour
{
    [SerializeField] private GameObject groupTypeMonsters;
    [SerializeField] private GameObject timeLine;
    bool isTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTrigger)
        {
            EventManager.Instance.SetSwitch(GameSwitch.isMainStoryOff, false);
            isTrigger = true;
            DialogueManager.Instance.StartStory(4);
            SystemMsg.Instance.UpdateMessage(9);
            timeLine.SetActive(true);

            DialogueManager.Instance.set.ui.playEvent += MonsterSpawn;
            Quest.Instance.NextQuest(6);
        }
    }

    void MonsterSpawn()
    {
        groupTypeMonsters.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
