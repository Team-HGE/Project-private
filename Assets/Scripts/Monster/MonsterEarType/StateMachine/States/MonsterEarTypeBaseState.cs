using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeBaseState : IState
{
    protected MonsterEarTypeStateMachine stateMachine;
    protected readonly MonsterGroundData groundData;

    public Vector3 noisePosition;
    private float _biggestNoise;



    public MonsterEarTypeBaseState(MonsterEarTypeStateMachine monsterStateMachine)
    {
        stateMachine = monsterStateMachine;
        groundData = stateMachine.Monster.Data.GroundData;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void HandleInput()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        if (!stateMachine.IsSearchTarget) SearchTarget();
        //SearchTarget();              
    }

    public virtual void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        //Debug.Log(movementDirection);
        Move(movementDirection);
        Rotate(movementDirection);
    }


    public virtual void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Monster.Controller.Move(((movementDirection * movementSpeed) + stateMachine.Monster.ForceReceiver.Movement) * Time.deltaTime);
    }

    // 수직, 수평 가하는 힘 
    protected void ForceMove()
    {
        stateMachine.Monster.Controller.Move(stateMachine.Monster.ForceReceiver.Movement * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        //Debug.Log(movementSpeed);

        return movementSpeed;
    }


    // 모든 상태에 필요한 애니메이션 전환 기능
    // 애니메이션 재생
    protected void StartAnimation(int animationHash)
    {
        //stateMachine.Monster.Animator.SetBool(animationHash, true);//구현 예정***
    }

    // 애니메이션 종료
    protected void StopAnimation(int animationHash)
    {
        //stateMachine.Monster.Animator.SetBool(animationHash, false);//구현 예정***
    }

    // 애니메이션 진행도 체크//수정 필요, 구현 예정***
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // 전환되고 있을때 && 다음 애니메이션 tag
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        // 전환되고 있지 않을때 && 현재 애니메이션 tag        
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    private void SearchTarget()
    {
        if (stateMachine.IsChasing) return;

        // 탐지 범위 SO에 추가***s
        stateMachine.Monster.noiseMakers = Physics.OverlapSphere(stateMachine.Monster.transform.position, 100f, stateMachine.Monster.targetLayer);

        if (stateMachine.Monster.noiseMakers.Length > 0)
        {

            for (int i = 0; i < stateMachine.Monster.noiseMakers.Length; i++)
            {
                // 플레이어 일때
                if (stateMachine.Monster.noiseMakers[i].tag == "Player")
                {

                    if (stateMachine.IsFocusNoise && _biggestNoise >= 5)
                    {
                        stateMachine.CurDestination = noisePosition;

                        // 추적
                        stateMachine.ChangeState(stateMachine.ChaseState);
                    }

                    //Debug.Log("플레이어 발견");
                    if (stateMachine.IsFocusRotate && stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<Player>().CurNoiseAmount > 0.96f && stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<Player>().CurNoiseAmount < 5f)
                    {
                        Rotate(GetMovementDirection());
                    }                    

                    if (CheckNoise(stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<Player>().CurNoiseAmount))
                    {
                        noisePosition = stateMachine.Monster.noiseMakers[i].gameObject.transform.position;

                    }

                }
                // 다른 오브젝트일때 , 태그, 레이어 중 선택할것***


                
                // 플레이어가 아닌 소음
                if (stateMachine.IsFocusNoise && _biggestNoise >= 5 && stateMachine.Monster.noiseMakers[i].tag != "Player")
                {
                    stateMachine.CurDestination = noisePosition;

                    // 추적
                    stateMachine.ChangeState(stateMachine.MoveState);
                }
            }

            if (!stateMachine.IsFocusNoise && _biggestNoise >= 12f)
            {
                stateMachine.CurDestination = noisePosition;

                _biggestNoise = 0f;
                noisePosition = Vector3.zero;

                // 이동 
                stateMachine.ChangeState(stateMachine.MoveState);
                //stateMachine.IsSearchTarget = true;
            }

            
        }
    }

    private bool CheckNoise(float curNoise)
    {
        if (_biggestNoise < curNoise)
        {
            _biggestNoise = curNoise;
            return true;
        }
        else { return false; }
    }

    protected bool IsInChaseRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.PlayerChasingRange * groundData.PlayerChasingRange;
    }

    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.AttackRange * groundData.AttackRange;
    }

    protected bool GetIsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = stateMachine.Target.transform.position - stateMachine.Monster.transform.position;
        float angle = Vector3.Angle(stateMachine.Monster.transform.forward, directionToPlayer);
        return angle < groundData.ViewAngle * 0.5f;
    }

    protected Vector3 GetMovementDirection()
    {
        if (stateMachine.IsMove)
        {
            Vector3 noiseDir = (stateMachine.CurDestination - stateMachine.Monster.transform.position).normalized;
            return noiseDir;

        }

        if (stateMachine.IsComeBack)
        {
            Vector3 backDir = (stateMachine.StartPosition - stateMachine.Monster.transform.position).normalized;
            return backDir;
        }       

        Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).normalized;

        return dir;
    }

    protected void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            stateMachine.Monster.transform.rotation = Quaternion.Lerp(stateMachine.Monster.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }
}
