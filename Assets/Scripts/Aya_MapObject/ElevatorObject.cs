using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ElevatorObject : MonoBehaviour
{
    Coroutine fixPlayerYPosCor;
    IEnumerator fixPlayerYPos(float playerDistance)
    {
        while (true)
        {
            Vector3 targetPos = GameManager.Instance.player.transform.position;
            targetPos.y = transform.position.y - playerDistance;
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
    float initialY;
    int lastUpdatedFloor = -1;

    [Header("FloorImage")]
    [SerializeField] Sprite[] elevatorSprites;
    [SerializeField] SpriteRenderer elevatorCountSprite;
    public event Action onInteractComplete;

    public int elevatorIndex;
    [SerializeField] DOTweenAnimation[] openDoor;
    [SerializeField] DOTweenAnimation[] closeDoor;

    
    [SerializeField] GameObject elevatorBoxCollider;

    public void MoveFloor(int targetFloor)
    {
        elevatorBoxCollider.SetActive(true);

        GameManager.Instance.player.transform.SetParent(this.transform);

        float targetY = (targetFloor - 1) * floorHeight;

        initialY = transform.position.y;
        
        int floorsToMove = Mathf.Abs(targetFloor - NowFloor);

        float allMoveTime = floorsToMove * moveTime;
        Tween moveUp = transform.DOMoveY(targetY, allMoveTime).SetEase(Ease.InOutQuad);
        moveUp.OnUpdate(() =>
        {
            float currentY = transform.position.y;
            int floor = Mathf.FloorToInt((currentY + floorHeight / 2) / floorHeight) + 1;

            if (floor != lastUpdatedFloor)
            {
                lastUpdatedFloor = floor;
                DOVirtual.DelayedCall(0.5f, () => UpdateElevatorCountSprite(floor));
            }
        });

        Sequence elevatorSequence = DOTween.Sequence();
        elevatorSequence.PrependCallback(() =>
        {
            CloseDoor();
            elevatorSequence.AppendInterval(5f);
        });
        elevatorSequence.Append(moveUp);
        //elevatorSequence.Join(transform.DOShakeRotation(allMoveTime, 1, 2).SetDelay(1));

        elevatorSequence.AppendCallback(() =>
        {
            HotelFloorScene_DataManager.Instance.elevatorManager.openTime = 0;
            GameManager.Instance.player.transform.SetParent(null);
            NowFloor = targetFloor;
            elevatorBoxCollider.SetActive(false);
            DOVirtual.DelayedCall(1f, () =>  OpenDoor());
        });
    }

    private void UpdateElevatorCountSprite(int floor)
    {
        elevatorCountSprite.sprite = elevatorSprites[floor];
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
        DOVirtual.DelayedCall(1f, () => onInteractComplete?.Invoke());
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
    }
}
 
