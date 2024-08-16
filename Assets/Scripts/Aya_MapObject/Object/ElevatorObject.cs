using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum ElevatorSoundType
{
    OpenSound,
    CloseSound,
    UpDownSound,
    BtnPressed
}

[Serializable]
public class ElevatorSound
{
    public ElevatorSoundType elevatorSoundType;
    public AudioClip audioClip;
}
public class ElevatorObject : MonoBehaviour
{
    Coroutine fixPlayerYPosCor;
    public bool isPlayerIn { get; set; }
    IEnumerator fixPlayerYPos()
    {
        while (true)
        {
            Vector3 targetPos = GameManager.Instance.player.transform.position;
            targetPos.y = transform.position.y + HotelFloorScene_DataManager.Instance.elevatorManager.playerHeightY;
            GameManager.Instance.player.transform.position = targetPos;
            yield return null;
        }
    }

    private int _nowFloor = 1;
    public int NowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }
    public float floorHeight = 35.5f;
    public float moveTime = 4f;
    int lastUpdatedFloor = -1;

    [Header("FloorImage")]
    [SerializeField] Sprite[] elevatorSprites;
    [SerializeField] SpriteRenderer[] elevatorCountSprite;
    public event Action onInteractComplete;

    public ElevatorObjectType elevatorObjectType;
    [SerializeField] DOTweenAnimation[] openDoor;
    [SerializeField] DOTweenAnimation[] closeDoor;

    [SerializeField] GameObject elevatorBoxCollider;

    [Header("Sound")]
    public AudioSource audioSource;
    public ElevatorSound[] elevatorSounds;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void MoveFloor(int targetFloor, bool isPlayerIn)
    {
        this.isPlayerIn = isPlayerIn;
        elevatorBoxCollider.SetActive(true);
        
        if (isPlayerIn)
        {
            fixPlayerYPosCor = StartCoroutine(fixPlayerYPos());
        }

        float targetY;
        if (targetFloor > 0)
        {
            targetY = (targetFloor - 1) * floorHeight;
            if (NowFloor < 0)
            {
                targetY += 106.5f;
            }
        }
        else if (targetFloor < 0)
        {
            targetY = -182;
        }
        else
        {
            targetY = -106.5f;
        }
        
        int floorsToMove = Mathf.Abs(targetFloor - NowFloor);

        float allMoveTime = floorsToMove * moveTime;

        if (targetFloor * NowFloor == 0)
        {
            allMoveTime += 3f;
        }
        else if (targetFloor * NowFloor < 0)
        {
            allMoveTime += 6f;
        }

        Tween moveUp = transform.DOLocalMoveY(targetY, allMoveTime).SetEase(Ease.InOutQuad);
        moveUp.OnUpdate(() =>
        {
            float currentY = transform.position.y;
            int floor = Mathf.RoundToInt(currentY / floorHeight) + 1;

            if (floor != lastUpdatedFloor)
            {
                if (floor <= 0)
                {
                    floor = 0;
                }
                lastUpdatedFloor = floor;
                DOVirtual.DelayedCall(0.5f, () => UpdateElevatorCountSprite(floor));
            }
        });

        Sequence elevatorSequence = DOTween.Sequence();
        elevatorSequence.AppendCallback(() =>
        {
            CloseDoor();

            audioSource.clip = elevatorSounds[(int)ElevatorSoundType.UpDownSound].audioClip;
            audioSource.Play();
            audioSource.loop = true;
        });
        elevatorSequence.AppendInterval(4f);
        elevatorSequence.Append(moveUp);
        elevatorSequence.Join(transform.DOShakeRotation(allMoveTime, 1, 2).SetDelay(1));
        elevatorSequence.AppendCallback(() =>
        {
            audioSource.Stop();
            if (isPlayerIn) StopCoroutine(fixPlayerYPosCor);
            HotelFloorScene_DataManager.Instance.elevatorManager.openTime = 0;
            NowFloor = targetFloor;
            elevatorBoxCollider.SetActive(false);
            DOVirtual.DelayedCall(1f, () =>  OpenDoor());
        });
    }

    private void UpdateElevatorCountSprite(int floor)
    {
        // 지하일경우 다른 배열 사용 필요
        foreach (var sprite in elevatorCountSprite)
        {
            sprite.sprite = elevatorSprites[floor];
        }
    }
    public void OpenDoor()
    {
        HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorOpen = true;
        List<DOTweenAnimation> nowDoor = HotelFloorScene_DataManager.Instance.elevatorManager.GetOpenDoor(NowFloor, this);
        
        foreach (var anim in nowDoor)
        {
            anim.DOKill();
            anim.CreateTween(true);
        }
        foreach (var anim in openDoor)
        {

            anim.DOKill();
            anim.CreateTween(true);
        }
        audioSource.PlayOneShot(elevatorSounds[(int)ElevatorSoundType.OpenSound].audioClip);
        DOVirtual.DelayedCall(4f, () => onInteractComplete?.Invoke());
    }

    public void CloseDoor()
    {
        HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorOpen = false;
        List<DOTweenAnimation> nowDoor = HotelFloorScene_DataManager.Instance.elevatorManager.GetCloseDoor(NowFloor, this);

        foreach (var anim in nowDoor)
        {
            anim.DOKill();
            anim.CreateTween(true);
        }
        foreach (var anim in closeDoor)
        {
            anim.DOKill();
            anim.CreateTween(true);
        }
        audioSource.PlayOneShot(elevatorSounds[(int)ElevatorSoundType.CloseSound].audioClip);
    }
}
 
