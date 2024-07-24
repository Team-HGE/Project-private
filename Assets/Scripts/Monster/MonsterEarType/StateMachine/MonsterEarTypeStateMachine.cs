using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeStateMachine : StateMachine
{
    public EarTypeMonster Monster { get; }

    // ���� ���, �÷��̾�
    public GameObject Target { get; private set; }
    // ���� ���� ��ġ
    public Vector3 StartPosition { get; private set; }

    // ���� ����***
    public MonsterEarTypeIdleState IdleState { get; private set; }
    public MonsterEarTypePatrolState PatrolState { get; private set; }
    public MonsterEarTypeMoveState MoveState { get; private set; }
    public MonsterEarTypeFocusState FocusState { get; private set; }
    public MonsterEarTypeComeBackState ComeBackState { get; private set; }
    public MonsterEarTypeChaseState ChaseState { get; private set; }

    // ���� ��ȯ ����
    public bool IsChasing { get; set; }
    public bool IsPatrol { get; set; }
    public bool IsSearchTarget { get; set; } = false;
    public bool IsFocusNoise { get; set; } = false;
    public bool IsMove { get; set; }
    public bool IsFocusRotate { get; set; }
    public bool IsComeBack { get; set; }
 
    public Vector3 CurDestination { get; set; }
    public float BiggestNoise { get; set; }


    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }

    public float MovementSpeedModifier { get; set; } = 1f;

    public bool IsPlayerInFieldOfView { get; set; }


    public MonsterEarTypeStateMachine(EarTypeMonster monster)
    {
        Monster = monster;
        // �±׷� �÷��� Ž��***
        Target = GameObject.FindGameObjectWithTag("Player");
        // ������ ��ġ
        StartPosition = Monster.transform.position;

        IdleState = new MonsterEarTypeIdleState(this);
        PatrolState = new MonsterEarTypePatrolState(this);
        MoveState = new MonsterEarTypeMoveState(this);
        FocusState = new MonsterEarTypeFocusState(this);
        ComeBackState = new MonsterEarTypeComeBackState(this);
        ChaseState = new MonsterEarTypeChaseState(this);

        MovementSpeed = Monster.Data.GroundData.BaseSpeed;
        RotationDamping = Monster.Data.GroundData.BaseRotationDamping;
    }
}
