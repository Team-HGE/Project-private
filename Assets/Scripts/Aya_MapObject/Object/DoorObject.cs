using UnityEngine;
using DG.Tweening;

public class DoorObject : InteractableObject
{
    [Header("Bool")]
    [SerializeField] bool isOpen;
    public bool isLock;

    [Header("SoundClip")]
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] AudioClip lockSound;

    [Header("Animation")]
    [SerializeField] DOTweenAnimation openDoor;
    [SerializeField] DOTweenAnimation closeDoor;
    [SerializeField] DOTweenAnimation lockDoor;

    AudioSource audioSource;
    AudioClip targetSound;

    void Awake()
    {
        // �ʿ��� ��� AudioSource ������Ʈ�� �ڵ����� �߰��մϴ�.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Start()
    {
        HotelFloorScene_DataManager.Instance.controller.doorObjects.Add(this);
    }

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "����/�ݱ�";
    }

    public override void Interact()
    {
        audioSource.volume = 1f;
        if (isLock)
        {
            openDoor.DOKill();
            closeDoor.DOKill();
            lockDoor.CreateTween(true);
            audioSource.volume = 0.5f;
            targetSound = lockSound;
        }

        if (isOpen && !isLock)
        {
            lockDoor.DOKill();
            openDoor.DOKill();
            closeDoor.CreateTween(true);
            targetSound = closeSound;
            isOpen = false;
        }
        else if (!isOpen && !isLock)
        {
            lockDoor.DOKill();
            closeDoor.DOKill();
            openDoor.CreateTween(true);
            targetSound = openSound;
            isOpen = true;
        }

        audioSource.PlayOneShot(targetSound);
    }
}