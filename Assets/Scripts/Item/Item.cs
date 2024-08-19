using System;
using UnityEngine;
public class Item : InteractableObject
{
    public event Action OnGetItem;

    // 첫날 컷신 
    private bool _isFisrtCutScene = false;

    public bool IsFisrtCutScene
    {
        get { return _isFisrtCutScene; }
        set
        {
            if (_isFisrtCutScene != value)
            {
                _isFisrtCutScene = value;
                OnGetItem?.Invoke();
            }
            else _isFisrtCutScene = value;
        }
    }


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
        GetEventItem();

        Destroy(gameObject);
    }

    private void GetEventItem()
    {
        if (gameObject.tag == "FirstCutScene")
        {
            EventManager.Instance.SetSwitch(GameSwitch.isMainStoryOff, true);
            GameDataSaveLoadManager.Instance.SaveGameData(0);
            IsFisrtCutScene = true;
        }
    }
}