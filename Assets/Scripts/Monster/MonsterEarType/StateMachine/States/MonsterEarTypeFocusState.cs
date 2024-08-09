﻿using System.Collections;
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
        stateMachine.BeforeNoise = 0f;
        stateMachine.IsFocusNoise = true;
        stateMachine.IsFocusRotate = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.FocusTransitionTime);

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.FocusParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("focus 끝");
        stateMachine.IsFocusNoise = false;
        stateMachine.IsFocusRotate = false;
        stateMachine.Monster.Agent.isStopped = false;

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.FocusParameterHash);
        stateMachine.Monster.StopWait();
        //stateMachine.Monster.IsBehavior = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.IsBehavior)
        {
            Debug.Log("집중 -> 복귀");

            stateMachine.ChangeState(stateMachine.ComeBackState);
            return;
        }

        //CheckNoise();
    }

    private void CheckNoise()
    {
        if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) > stateMachine.Monster.Data.GroundData.PlayerChasingRange) return;

        for (int i = 0; i < stateMachine.Monster.noiseMakers.Count; i++)
        {
            if (stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount >= 0.95f && stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount < 5.5f)
            {
                stateMachine.CurDestination = stateMachine.Monster.noiseMakers[i].gameObject.transform.position;
                Rotate(GetMovementDirection());
            }   

            if (stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount >= 5.5f)
            {
                stateMachine.CurDestination = stateMachine.Monster.noiseMakers[i].gameObject.transform.position;

                if (stateMachine.Monster.noiseMakers[i].tag == "Player")
                {
                    //Debug.Log("플레이어 추적");

                    stateMachine.ChangeState(stateMachine.ChaseState);
                }

                if (stateMachine.Monster.noiseMakers[i].tag == "NoiseMaker")
                {
                    //Debug.Log("아이템 추적");

                    stateMachine.ChangeState(stateMachine.MoveState);
                }
            }
        }
    }

}
