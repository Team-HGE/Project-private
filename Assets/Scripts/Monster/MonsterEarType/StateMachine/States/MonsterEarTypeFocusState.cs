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

        Debug.Log("focus ����");

        stateMachine.IsFocusNoise = true;
        stateMachine.IsFocusRotate = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.FocusTransitionTime);

        // �ִϸ��̼� ����

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("focus ��");

        stateMachine.IsFocusNoise = false;
        stateMachine.IsFocusRotate = false;

        // �ִϸ��̼� ����

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.IsBehavior)
        {
            Debug.Log("����");

            // ����
            stateMachine.ChangeState(stateMachine.ComeBackState);
        }
    }

}
