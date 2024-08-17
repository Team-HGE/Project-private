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
            timeLine.SetActive(true);

            DialogueManager.Instance.set.ui.playEvent += MonsterSpawn;
        }
    }

    void MonsterSpawn()
    {
        groupTypeMonsters.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
