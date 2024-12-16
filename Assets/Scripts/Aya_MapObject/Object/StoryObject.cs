using UnityEngine;

public class StoryObject : MonoBehaviour
{
    public int index;
    [SerializeField] private bool isPlay = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && index > -1 && !isPlay)
        {
            isPlay = true;
            DialogueManager.Instance.StartStory(index);

            Destroy(gameObject);
        }
    }
}
