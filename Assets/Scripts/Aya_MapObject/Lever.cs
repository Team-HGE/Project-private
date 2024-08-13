using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
public class Lever : InteractableObject
{
    [Title("OnOffLight")]
    [SerializeField] Floor nowFloor;

    [Title("ObjectControll")]
    [SerializeField] DOTweenAnimation laverUp;

    [Title("Sound")]
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
        audioSource.Play();
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
