using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeBaseState : IState
{
    protected MonsterEarTypeStateMachine stateMachine;
    protected readonly MonsterGroundData groundData;

    public Vector3 noisePosition;

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
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }

    // 애니메이션 종료
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
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
        if (stateMachine.IsChasing || stateMachine.IsFocusNoise || stateMachine.IsAttack) return;
        //Debug.Log("SearchTarget");

        stateMachine.BiggestNoise = 0f;

        stateMachine.Monster.noiseMakers.Clear();

        Collider[] temp = Physics.OverlapSphere(stateMachine.Monster.transform.position, stateMachine.Monster.Data.GroundData.PlayerChasingRange * 2, stateMachine.Monster.targetLayer);

        foreach (Collider col in temp)
        {        
            if (col.tag == "NoiseMaker" || col.tag == "Player")
            {
                stateMachine.Monster.noiseMakers.Add(col);
                //CheckNoise(col.gameObject.GetComponent<INoise>().CurNoiseAmount);

                if (CheckNoise(col.gameObject.GetComponent<INoise>().CurNoiseAmount)) stateMachine.CurDestination = col.gameObject.transform.position;

            }
        }

        // 아이템, 장비 소음 발생***
        if (stateMachine.BiggestNoise >= 90f)
        {
            //Debug.Log("아이템 감지");
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange && stateMachine.BiggestNoise >= 11.5f)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }

        if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange * 0.5f && stateMachine.BiggestNoise >= 5.5f)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
    }

    protected bool CheckNoise(float curNoise)
    {
        if (stateMachine.BiggestNoise < curNoise)
        {
            stateMachine.BiggestNoise = curNoise;
            return true;
        }
        else return false;
    }

    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.AttackRange * groundData.AttackRange;
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
