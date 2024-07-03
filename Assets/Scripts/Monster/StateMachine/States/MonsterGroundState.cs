using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroundState : MonsterBaseState
{
    public MonsterGroundState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 애니메이션 실행
        //StartAnimation(stateMachine.Monster.AnimationData.GroundParameterHash);//구현 예정***
    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 종료
        //StopAnimation(stateMachine.Monster.AnimationData.GroundParameterHash);//구현 예정***
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
