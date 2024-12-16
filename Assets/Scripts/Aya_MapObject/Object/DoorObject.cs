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
        // 필요한 경우 AudioSource 컴포넌트를 자동으로 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Start()
    {
        HotelFloorScene_DataManager.Instance.controller.doorObjects.Add(this);
        if (EventManager.Instance.GetSwitch(GameSwitch.DoorUnlocked) && !this.gameObject.CompareTag("NoLock"))
        {
            isLock = EventManager.Instance.GetSwitch(GameSwitch.DoorUnlocked); 
        }
    }

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "열기/닫기";
    }

    public override void Interact()
    {
        audioSource.volume = 1f;
        if (isLock)
        {
            LockDoor();
        }

        if (isOpen && !isLock)
        {
            CloseDoor();
        }
        else if (!isOpen && !isLock)
        {
           
            OpenDoor();
        }

        audioSource.PlayOneShot(targetSound);
    }

    public void OpenDoor()
    {
        lockDoor.DOKill();
        closeDoor.DOKill();
        openDoor.CreateTween(true);
        targetSound = openSound;
        isOpen = true;
    }

    public void CloseDoor()
    {
        lockDoor.DOKill();
        openDoor.DOKill();
        closeDoor.CreateTween(true);
        targetSound = closeSound;
        isOpen = false;
    }

    public void LockDoor()
    {
        openDoor.DOKill();
        closeDoor.DOKill();
        lockDoor.CreateTween(true);
        audioSource.volume = 0.5f;
        targetSound = lockSound;
    }
}