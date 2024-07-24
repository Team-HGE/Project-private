using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public Monster Monster { get; }

    // 공격 대상, 플레이어
    public GameObject Target { get; private set; }
    // 몬스터 생성 위치
    public Vector3 StartPosition { get; private set; }

    // 몬스터 상태
    public MonsterIdleState IdleState { get; private set; }
    public MonsterPatrolState PatrolState { get; private set; }
    public MonsterFindState FindState { get; private set; }
    public MonsterChaseState ChaseState { get; private set; }
    public MonsterLoseSightState LoseSightState { get; private set; }
    public MonsterComeBackState ComBackState { get; private set; }
    public MonsterAttackState AttackState { get; private set; }


    // 상태 전환 조건
    public bool IsChasing { get; set; }
    public bool IsPatrol { get; set; }
    public bool IsAttack { get; set; }


    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    
    public float MovementSpeedModifier { get; set; } = 1f;

    public bool IsPlayerInFieldOfView { get; set; }

    public MonsterStateMachine(Monster monster)
    {
        Monster = monster;
        // 태그로 플레어 탐색
        Target = GameObject.FindGameObjectWithTag("Player");
        // 고유한 위치
        StartPosition = Monster.transform.position;

        IdleState = new MonsterIdleState(this);
        PatrolState = new MonsterPatrolState(this);
        FindState = new MonsterFindState(this);
        ChaseState = new MonsterChaseState(this);
        LoseSightState = new MonsterLoseSightState(this);
        ComBackState = new MonsterComeBackState(this);
        AttackState = new MonsterAttackState(this);

        MovementSpeed = Monster.Data.GroundData.BaseSpeed;
        RotationDamping = Monster.Data.GroundData.BaseRotationDamping;
    }
}
