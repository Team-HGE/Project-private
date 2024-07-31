using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeStateMachine : StateMachine
{
    public EarTypeMonster Monster { get; }

    // 공격 대상, 플레이어
    public GameObject Target { get; private set; }

    // 몬스터 생성 위치
    public Vector3 StartPosition { get; private set; }

    // 몬스터 상태***
    public MonsterEarTypeIdleState IdleState { get; private set; }
    public MonsterEarTypePatrolState PatrolState { get; private set; }
    public MonsterEarTypeMoveState MoveState { get; private set; }
    public MonsterEarTypeFocusState FocusState { get; private set; }
    public MonsterEarTypeComeBackState ComeBackState { get; private set; }
    public MonsterEarTypeChaseState ChaseState { get; private set; }
    public MonsterEarTypeAttackState AttackState { get; private set; }

    // 상태 전환 조건
    public bool IsPatrol { get; set; }
    //public bool IsSearchTarget { get; set; } = false;
    public bool IsMove { get; set; }
    public bool IsFocusNoise { get; set; } = false;
    public bool IsFocusRotate { get; set; }
    public bool IsChasing { get; set; }
    public bool IsComeBack { get; set; }
    public bool IsAttack { get; set; }

    // 추적
    public Vector3 CurDestination { get; set; }
    public float BiggestNoise { get; set; }
    public float BeforeNoise { get; set; } = 0f;

    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }

    //public float MovementSpeedModifier { get; set; } = 1f;

    public bool IsPlayerInFieldOfView { get; set; }


    public MonsterEarTypeStateMachine(EarTypeMonster monster)
    {
        Monster = monster;
        // 태그로 플레어 탐색***
        Target = GameObject.FindGameObjectWithTag("Player");
        // 고유한 위치
        StartPosition = Monster.transform.position;

        IdleState = new MonsterEarTypeIdleState(this);
        PatrolState = new MonsterEarTypePatrolState(this);
        MoveState = new MonsterEarTypeMoveState(this);
        FocusState = new MonsterEarTypeFocusState(this);
        ComeBackState = new MonsterEarTypeComeBackState(this);
        ChaseState = new MonsterEarTypeChaseState(this);
        AttackState = new MonsterEarTypeAttackState(this);

        MovementSpeed = Monster.Data.GroundData.BaseSpeed;
        RotationDamping = Monster.Data.GroundData.BaseRotationDamping;
    }
}
