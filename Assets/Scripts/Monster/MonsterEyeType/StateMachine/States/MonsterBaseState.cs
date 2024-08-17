using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : IState
{
    protected MonsterStateMachine stateMachine;
    protected readonly MonsterGroundData groundData;

    public MonsterBaseState(MonsterStateMachine monsterStateMachine)
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
        if (GameManager.Instance.nowPlayCutScene)
        {
            if (!stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = true;
            stateMachine.Monster.Agent.ResetPath();
            return;
        }
        else
        {
            if (stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = false;
        }

        RotateToPlayer();
        AttackToPlayer();

        FindPlayerCheck();
    }

    // 애니메이션 재생
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }

    // 애니메이션 종료
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
    }

    protected void RotateToPlayer()
    {
        if (IsInFeelRange())
        {
            Rotate(GetMovementDirection());
        }
    }

    protected void AttackToPlayer()
    {
        if (IsInAttackRange() && GetIsPlayerInFieldOfView())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }            
    }

    protected void FindPlayerCheck()
    {
        if (stateMachine.IsAttack) return;

        if (!stateMachine.Monster.canCheck)
        {
            //Debug.Log("플레이어 탐지 불가");
            if (stateMachine.Monster.canSeePlayer) stateMachine.Monster.canSeePlayer = false;
            return;
        }
            
        //if (stateMachine.Monster.rangeChecks.Length <= 0)
        //{
        //    //Debug.Log("플레이어 탐지 불가");
        //    if (stateMachine.Monster.canSeePlayer) stateMachine.Monster.canSeePlayer = false;
        //    return;
        //}

        //Transform target = stateMachine.Monster.rangeChecks[0].transform;
        Transform target = stateMachine.Monster.findTarget;
         
        Vector3 directionToTarget = (target.position - stateMachine.Monster.transform.position).normalized;
        Vector3 directionToTargetEye = (new Vector3(target.position.x, stateMachine.Monster.eye.position.y, target.position.z) - stateMachine.Monster.eye.position).normalized;


        if (Vector3.Angle(stateMachine.Monster.transform.forward, directionToTarget) < stateMachine.Monster.Data.GroundData.ViewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(stateMachine.Monster.transform.position, target.position);
            float distanceToTargetEye = Vector3.Distance(stateMachine.Monster.eye.position, new Vector3(target.position.x, stateMachine.Monster.eye.position.y, target.position.z));


            //Debug.Log($"FindPlayerCheck - 바닥, {Physics.Raycast(stateMachine.Monster .transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.playerMask)}, {stateMachine.Monster.transform.position}, {distanceToTarget} ");


            if (!Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask))
            {
                stateMachine.Monster.canSeePlayer = true;
                if (stateMachine.IsFind || stateMachine.IsChasing) return;
                else
                {
                    //Debug.Log($"FindPlayerCheck - 플레이어 발견 - 바닥, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.playerMask)}, {stateMachine.Monster.transform.position} ");

                    stateMachine.ChangeState(stateMachine.FindState);
                }
                
                return;
            }
            else stateMachine.Monster.canSeePlayer = false;

            //Debug.Log($"FindPlayerCheck - 눈, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.playerMask)}, {stateMachine.Monster.eye.position}, {distanceToTargetEye} ");


            if (!Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.obstructionMask) && Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.playerMask))
            {
                stateMachine.Monster.canSeePlayer = true;

                if (stateMachine.IsFind || stateMachine.IsChasing) return;
                else
                {
                    //Debug.Log($"FindPlayerCheck - 플레이어 발견 - 눈, {stateMachine.Monster.eye.position}");
                    //Debug.Log($"FindPlayerCheck - 플레이어 발견 - 눈, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.playerMask)}, {stateMachine.Monster.eye.position}, {distanceToTargetEye} ");
                    stateMachine.ChangeState(stateMachine.FindState);
                }

                return;
            }
            else stateMachine.Monster.canSeePlayer = false;

        }
        else stateMachine.Monster.canSeePlayer = false;

    }

    protected bool IsInChaseRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.PlayerChasingRange * groundData.PlayerChasingRange;
    }

    protected bool IsInFindRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.PlayerFindRange * groundData.PlayerFindRange;
    }

    protected bool IsInFeelRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.PlayerFeelRange * groundData.PlayerFeelRange;
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

    // 수직
    //protected void ForceMove()
    //{
    //    stateMachine.Monster.Controller.Move(stateMachine.Monster.ForceReceiver.Movement * Time.deltaTime);
    //}
}
