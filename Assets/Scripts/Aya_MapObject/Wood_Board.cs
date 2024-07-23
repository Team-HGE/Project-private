public class Wood_Board : InteractableObject
{
    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "B동으로 이동";
    }

    public override void Interact()
    {
        isInteractable = true;
        GameManager.Instance.fadeManager.MoveScene(SceneEnum.BScene);
    }
}
