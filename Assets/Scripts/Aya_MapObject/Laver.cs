using DG.Tweening;
using UnityEngine;
public class Laver : InteractableObject
{
    [Header("OnOffLight")]
    [SerializeField] Light[] nowFloorlights;
    [SerializeField] MeshRenderer[] nowFloorObjectRenderers;
    [SerializeField] GameObject[] nowFloorObject;

    [Header("ObjectControll")]
    [SerializeField] DOTweenAnimation laverUp;
    private void Start()
    {
        GameManager.Instance.lightManager.lavers.Add(this);
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
    public void OffNowFloorAllLight()
    {
        foreach (var light in nowFloorlights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
        foreach (var obj in nowFloorObject)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        GameManager.Instance.lightManager.ChangeMaterial(nowFloorObjectRenderers);
    }
    public void OnNowFloorAllLight()
    {
        foreach (var light in nowFloorlights)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }
        foreach (var obj in nowFloorObject)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        GameManager.Instance.lightManager.ChangeMaterial(nowFloorObjectRenderers);
    }
}
