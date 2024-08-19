using UnityEngine;

public class MonsterGroupTypeStateMachine : StateMachine
{
    public GroupTypeMonster Monster { get; }

    // 공격 대상, 플레이어
    public GameObject Target { get; private set; }

    // 몬스터 상태
    public MonsterGroupTypeChaseState ChaseState { get; private set; }
    public MonsterGroupTypeAttackState AttackState { get; private set; }

    // 현재 상태
    public bool IsChasing { get; set; }
    public bool IsAttack { get; set; }


    public float RotationDamping { get; private set; }


    public MonsterGroupTypeStateMachine(GroupTypeMonster monster)
    {
        Monster = monster;
        // 플레이어 캐싱
        Target = GameObject.FindGameObjectWithTag("Player");

        ChaseState = new MonsterGroupTypeChaseState(this);
        AttackState = new MonsterGroupTypeAttackState(this);

        RotationDamping = Monster.Data.GroundData.BaseRotationDamping;
    }
}
