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
        // 필요한 경우 AudioSource 컴포넌트를 자동으로 추가합니다.
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
        GameManager.Instance.player.interactableText.text = "열기/닫기";
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

        // 효과음 재생
        if (targetSound != null)
        {
            audioSource.PlayOneShot(targetSound);
        }
    }
}