using DG.Tweening;
using UnityEngine;

public class DrawerObject : InteractableObject
{
    [SerializeField] DOTweenAnimation openDrawer;
    [SerializeField] DOTweenAnimation closeDrawer;
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] GameObject item;

    bool isOpen { get; set; }

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);

        GameManager.Instance.player.interactableText.text = isOpen ? "´Ý±â" : "¿­±â";
    }

    public override void Interact()
    {
        if (isInteractable) return;
        isInteractable = true;

        if (isOpen)
        {
            audioSource.clip = closeSound;
            audioSource.Play();
            openDrawer.DOKill();
            closeDrawer.CreateTween(true);
            isOpen = false;
        }
        else
        {
            audioSource.clip = openSound;
            audioSource.Play();
            closeDrawer.DOKill();
            openDrawer.CreateTween(true);
            isOpen = true;
        }
    }

    public void FalseInteracte()
    {
        if (item != null)
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
            
        isInteractable = false;
    }
}
