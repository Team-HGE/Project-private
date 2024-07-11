using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Cinemachine;

public class ElevatorObject : MonoBehaviour
{
    [SerializeField] Transform elevatorLeftDoor;
    [SerializeField] Transform elevatorRightDoor;
    public event Action onInteractComplete;
    [SerializeField] GameObject elevatorBoxCollider;
    [SerializeField] Sprite[] elevatorSprites;
    [SerializeField] SpriteRenderer elevatorCountSprite;
    private CinemachineTransposer _virtualCamera;
    CinemachineTransposer virtualCamera
    {
        get
        {
            if (_virtualCamera == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("VCam");
                _virtualCamera = obj.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
            }
            return _virtualCamera;
        }
    }

    public void MoveElevatorTo(int targetFloor)
    {
        elevatorBoxCollider.SetActive(true);
        ElevatorDoorClose(targetFloor);
    }
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
    public void ElevatorUp(int targetFloor, bool init, float playerDistance)
    {
        float moveTime;
        Ease nowEase;
        if (init)
        {
            fixPlayerY(playerDistance);
            nowEase = Ease.InQuad;
            moveTime = 3f;
        }
        else
        {
            nowEase = Ease.Linear;
            moveTime = 4f;
            
        }
        transform.DOShakeRotation(moveTime, 1, 2).SetDelay(1);
        var nowMove = transform.DOMoveY(38, moveTime).SetEase(nowEase);

        nowMove.onComplete += ()=> 
        {
            Vector3 pos = transform.position;
            pos.y = -46;
            transform.position = pos;
            GameManager.Instance.nowFloor++;
            elevatorCountSprite.sprite = elevatorSprites[GameManager.Instance.nowFloor];
            if (targetFloor <= GameManager.Instance.nowFloor)
            {
                transform.DOShakeRotation(3, 1, 2);
                transform.DOMoveY(0, 3).onComplete += () =>
                {
                    ReleasePlayerY();
                    ElevatorDoorOpen();
                };
            }
            else { ElevatorUp(targetFloor, false, playerDistance);}
        };
    }
    public void ElevatorDown(int targetFloor, bool init, float playerDistance)
    {
        float moveTime;
        Ease nowEase;
        if (init)
        {
            fixPlayerY(playerDistance);
            nowEase = Ease.InQuad;
            moveTime = 3f;
        }
        else
        {
            nowEase = Ease.Linear;
            moveTime = 4f;

        }
        transform.DOShakeRotation(moveTime, 1, 2).SetDelay(1);
        var nowMove = transform.DOMoveY(-46, moveTime).SetEase(nowEase);

        nowMove.onComplete += () =>
        {
            Vector3 pos = transform.position;
            pos.y = 38;
            transform.position = pos;
            GameManager.Instance.nowFloor--;
            if (targetFloor >= GameManager.Instance.nowFloor)
            {
                transform.DOShakeRotation(3, 1, 2);
                transform.DOMoveY(0, 3).onComplete += () =>
                {
                    ReleasePlayerY();
                    ElevatorDoorOpen();
                };
            }
            else { ElevatorDown(targetFloor, false, playerDistance); }
        };
    }

    public void ElevatorDoor(int open)
    {
        if (open == 0)
        {
            ElevatorDoorOpen();
        }
    }
    public void ElevatorDoorOpen()
    {
        elevatorBoxCollider.SetActive(false);
        elevatorLeftDoor.DOLocalMoveX(5,3).SetDelay(1);
        elevatorRightDoor.DOLocalMoveX(-17.5f,3).SetDelay(1).onComplete +=() => 
        {
            onInteractComplete?.Invoke();
        }; 
    }
    public void ElevatorDoorClose(int targetFloor)
    {
        elevatorLeftDoor.DOLocalMoveX(-2, 3).SetDelay(1);
        elevatorRightDoor.DOLocalMoveX(-10.5f, 3).SetDelay(1).onComplete += () => 
        {
            if (targetFloor == -100)
            {
                return;
            }

            if (GameManager.Instance.nowFloor < targetFloor)
            {
                ElevatorUp(targetFloor, true, transform.position.y - GameManager.Instance.player.transform.position.y);
            }
            else if (GameManager.Instance.nowFloor > targetFloor)
            {
                ElevatorDown(targetFloor, true, transform.position.y - GameManager.Instance.player.transform.position.y);
            }
            
        };
    }
    Vector3 oringDamping = Vector3.zero;
    void fixPlayerY(float playerDistance)
    {
        fixPlayerYPosCor = StartCoroutine(fixPlayerYPos(playerDistance));
        oringDamping.x = virtualCamera.m_XDamping;
        oringDamping.y = virtualCamera.m_YDamping;
        oringDamping.z = virtualCamera.m_ZDamping;

        virtualCamera.m_XDamping = 0;
        virtualCamera.m_YDamping = 0;
        virtualCamera.m_ZDamping = 0;
    }
    void ReleasePlayerY()
    {
        StopCoroutine(fixPlayerYPosCor);
        virtualCamera.m_XDamping = oringDamping.x;
        virtualCamera.m_YDamping = oringDamping.y;
        virtualCamera.m_ZDamping = oringDamping.z;
    }
    
}
