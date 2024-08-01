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
        FindPlayerCheck();
    }

    
    public virtual void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Monster.Controller.Move(((movementDirection * movementSpeed) + stateMachine.Monster.ForceReceiver.Movement) * Time.deltaTime);
    }

    // ����, ���� ���ϴ� �� 
    protected void ForceMove()
    {
        stateMachine.Monster.Controller.Move(stateMachine.Monster.ForceReceiver.Movement * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }


    // ��� ���¿� �ʿ��� �ִϸ��̼� ��ȯ ���
    // �ִϸ��̼� ���
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }

    // �ִϸ��̼� ����
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
    }

    // �ִϸ��̼� ���൵ üũ
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        // ��ȯ�ǰ� ������ && ���� �ִϸ��̼� tag
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        // ��ȯ�ǰ� ���� ������ && ���� �ִϸ��̼� tag        
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected void FindPlayerCheck()
    {
        if (!stateMachine.Monster.canCheck)
        {
            //Debug.Log("�÷��̾� Ž�� �Ұ�");
            if (stateMachine.Monster.canSeePlayer) stateMachine.Monster.canSeePlayer = false;
            return;
        }
            
        //if (stateMachine.Monster.rangeChecks.Length <= 0)
        //{
        //    //Debug.Log("�÷��̾� Ž�� �Ұ�");
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


            Debug.Log($"FindPlayerCheck - �ٴ�, {Physics.Raycast(stateMachine.Monster .transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.playerMask)}, {stateMachine.Monster.transform.position}, {distanceToTarget} ");


            if (!Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask))
            {
                stateMachine.Monster.canSeePlayer = true;
                if (stateMachine.IsFocus || stateMachine.IsChasing) return;
                else
                {
                    Debug.Log($"FindPlayerCheck - �÷��̾� �߰� - �ٴ�, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.transform.position, directionToTarget, distanceToTarget, stateMachine.Monster.playerMask)}, {stateMachine.Monster.transform.position} ");

                    stateMachine.ChangeState(stateMachine.FindState);
                }
                
                return;
            }
            else stateMachine.Monster.canSeePlayer = false;

            Debug.Log($"FindPlayerCheck - ��, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.obstructionMask)}, {Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.playerMask)}, {stateMachine.Monster.eye.position}, {distanceToTargetEye} ");


            if (!Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.obstructionMask) && Physics.Raycast(stateMachine.Monster.eye.position, directionToTargetEye, distanceToTargetEye, stateMachine.Monster.playerMask))
            {
                stateMachine.Monster.canSeePlayer = true;

                if (stateMachine.IsFocus || stateMachine.IsChasing) return;
                else
                {
                    Debug.Log($"FindPlayerCheck - �÷��̾� �߰� - ��, {stateMachine.Monster.eye.position}");
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
}
