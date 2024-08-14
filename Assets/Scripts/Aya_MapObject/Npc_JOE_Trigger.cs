using UnityEngine;

public class Npc_JOE_Trigger : MonoBehaviour
{
    [SerializeField] private GameObject timeLine;
    bool isTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTrigger = true;
            DialogueManager.Instance.StartStory(4);
            timeLine.SetActive(true);
        }
    }
}
