using DG.Tweening;
using UnityEngine;
public class Lever : InteractableObject
{
    [Header("OnOffLight")]
    [SerializeField] Floor nowFloor;

    [Header("ObjectControll")]
    [SerializeField] DOTweenAnimation laverUp;
    private void Start()
    {
        nowFloor = FloorInitializer.Instance.ReturnFloorPosition(transform.position);
        GameManager.Instance.lightManager.levers.Add(this);
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "¿Ã¸®±â";
        
    }
    public override void Interact()
    {
        if (isInteractable) return;
        laverUp.DOPlay();
        isInteractable = true;
    }

    public void OnNowFloorAllLight()
    {
        GameManager.Instance.lightManager.SetFloorPowerStatus(nowFloor, true);

        GameManager.Instance.lightManager.OnListLight(GameManager.Instance.lightManager.GetLightsForFloor(nowFloor));

        GameManager.Instance.lightManager.OnChangeMaterial(GameManager.Instance.lightManager.GetRenderersForFloor(nowFloor));
    }
    public void OffNowFloorAllLight()
    {
        GameManager.Instance.lightManager.SetFloorPowerStatus(nowFloor, false);

        GameManager.Instance.lightManager.OffListLight(GameManager.Instance.lightManager.GetLightsForFloor(nowFloor));

        GameManager.Instance.lightManager.OffChangeMaterial(GameManager.Instance.lightManager.GetRenderersForFloor(nowFloor));
    }
}
