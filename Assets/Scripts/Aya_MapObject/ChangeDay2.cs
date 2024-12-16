using UnityEngine;

public class ChangeDay2 : MonoBehaviour
{
    [SerializeField] ScriptSO scriptSO;

    private void OnTriggerEnter(Collider other)
    {
        DialogueManager.Instance.StartStory(2);
    }
}
