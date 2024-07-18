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
            return;
        }

        CheckNoise();
    }

    private void CheckNoise()
    {
        if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) > stateMachine.Monster.Data.GroundData.PlayerChasingRange) return;

        // �ȴ� �Ҹ����� ����
        for (int i = 0; i < stateMachine.Monster.noiseMakers.Count; i++)
        {
            if (stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount >= 0.95f && stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount < 5.5f)
            {
                stateMachine.CurDestination = stateMachine.Monster.noiseMakers[i].gameObject.transform.position;
                Rotate(GetMovementDirection());
            }   

            // ���� �߻�
            if (stateMachine.Monster.noiseMakers[i].gameObject.GetComponent<INoise>().CurNoiseAmount >= 5.5f)
            {
                stateMachine.CurDestination = stateMachine.Monster.noiseMakers[i].gameObject.transform.position;

                //�÷��̾��϶�
                if (stateMachine.Monster.noiseMakers[i].tag == "Player")
                {
                    Debug.Log("�÷��̾� ����");

                    stateMachine.ChangeState(stateMachine.ChaseState);
                }

                if (stateMachine.Monster.noiseMakers[i].tag == "NoiseMaker")
                {
                    Debug.Log("������ ����");

                    stateMachine.ChangeState(stateMachine.MoveState);
                }
            }
        }
    }

}
