using UnityEngine;

public class FirstCutScene : MonoBehaviour
{
    public GameObject TLTrigger;
    // 획득 아이템
    public GameObject eventObject;
    //public bool onTrigger;

    private FirstTLTrigger trigger;
    private FirstCutSceneEvent sceneEvent;
    private Item item;

    private void Start()
    {
        trigger = TLTrigger.GetComponent<FirstTLTrigger>();
        trigger.OnEnd += HandleEnd;
        
        item = eventObject.GetComponent<Item>();
        item.OnGetItem += HandleGetItem;

        sceneEvent = GetComponent<FirstCutSceneEvent>();
    }

    private void HandleGetItem()
    {
        if (item.IsFisrtCutScene) TLTrigger.SetActive(true);
    }

    private void HandleEnd()
    {
        if (trigger.IsEnd)
        {
            TLTrigger.SetActive(false);
            sceneEvent.EventOn();
        }
    }

    private void OnDestroy()
    {
        trigger.OnEnd -= HandleEnd;
        item.OnGetItem -= HandleGetItem;
    }
}
