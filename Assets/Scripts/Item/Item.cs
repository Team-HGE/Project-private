using UnityEngine;

public class Item : InteractableObject
{
    //public ItemSO itemSO;
    public ScriptSO scriptSO;

    //private void Init(ItemSO _item)
    //{
    //    itemSO = _item;
    //}

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "수집";

    }

    public override void Interact()
    {
        //Init(itemSO);

        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        Debug.Log("아이템 파괴");
        Destroy(gameObject);
    }
}