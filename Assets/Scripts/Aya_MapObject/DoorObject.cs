using UnityEngine;
using DG.Tweening;

public class DoorObject : InteractableObject
{
    [SerializeField] bool isOpen;
    [SerializeField] bool interactableOneTime;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] DOTweenAnimation openDoor;
    [SerializeField] DOTweenAnimation closeDoor;

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
        GameManager.Instance.player.interactableText.text = "����/�ݱ�";
    }

    public override void Interact()
    {
        if (interactableOneTime)
        {
            isInteractable = true;
        }

        AudioClip targetSound;

        
        if (isOpen)
        {
            openDoor.DOKill();
            closeDoor.CreateTween(true);
            targetSound = closeSound;
            isOpen = false;
        }
        else
        {
            closeDoor.DOKill();
            openDoor.CreateTween(true);
            targetSound = openSound;
            isOpen = true;
        }

        // ȿ���� ���
        if (targetSound != null)
        {
            audioSource.PlayOneShot(targetSound);
        }
    }
}