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
