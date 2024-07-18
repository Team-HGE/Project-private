using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeFocusState : MonsterEarTypeGroundState
{
    public MonsterEarTypeFocusState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("focus 시작");

        stateMachine.IsFocusNoise = true;
        stateMachine.IsFocusRotate = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.FocusTransitionTime);

        // 애니메이션 실행

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("focus 끝");

        stateMachine.IsFocusNoise = false;
        stateMachine.IsFocusRotate = false;

        // 애니메이션 종료

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.IsBehavior)
        {
            Debug.Log("복귀");

            // 복귀
            stateMachine.ChangeState(stateMachine.ComeBackState);
        }
    }

}
