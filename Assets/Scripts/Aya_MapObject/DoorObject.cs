using UnityEngine;
using DG.Tweening;

public class DoorObject : InteractableObject
{
    [SerializeField] bool isOpen;
    [SerializeField] bool interactableOneTime;
    [SerializeField] Vector3 openRotate;
    [SerializeField] Vector3 closeRotate;
    [SerializeField] Transform parentObject;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;

    private AudioSource audioSource;

    void Awake()
    {
        // �ʿ��� ��� AudioSource ������Ʈ�� �ڵ����� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] Door Open";
    }

    public override void Interact()
    {
        if (interactableOneTime)
        {
            isInteractable = true;
        }

        Vector3 targetRotate;
        AudioClip targetSound;

        if (isOpen)
        {
            targetRotate = closeRotate;
            targetSound = closeSound;
            isOpen = false;
        }
        else
        {
            targetRotate = openRotate;
            targetSound = openSound;
            isOpen = true;
        }

        parentObject.DOKill();
        parentObject.DORotate(targetRotate, 2f);

        // ȿ���� ���
        if (targetSound != null)
        {
            audioSource.PlayOneShot(targetSound);
        }
    }
}