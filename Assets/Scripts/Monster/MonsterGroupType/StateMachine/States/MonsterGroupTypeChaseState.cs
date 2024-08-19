using UnityEngine;
using UnityEngine.AI;

public class MonsterGroupTypeChaseState : MonsterGroupTypeGroundState
{

    private float delay = 0.5f;
    private float delayTimer = 0f;


    public MonsterGroupTypeChaseState(MonsterGroupTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("플레이어 추적 시작");
        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("플레이어 추적 종료");
        stateMachine.IsChasing = false;
        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        base.Update();

        Rotate(GetMovementDirection());

        if (GetIsPlayerInFieldOfView() && IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            delayTimer = 0f;
            ChaseCheck();
        }
    }

    private void ChaseCheck()
    {
        if (stateMachine.Monster.Agent.pathPending) return;

        stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);

        if (NavMesh.SamplePosition(stateMachine.Target.transform.position, out NavMeshHit hit, 1000f, NavMesh.AllAreas))
        {
            stateMachine.Monster.Agent.SetDestination(hit.position);

            return;
        }
    }
}
