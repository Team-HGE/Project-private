using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterEarTypeBaseState : IState
{
    protected MonsterEarTypeStateMachine stateMachine;
    protected readonly MonsterGroundData groundData;

    public Vector3 noisePosition;

    public MonsterEarTypeBaseState(MonsterEarTypeStateMachine monsterStateMachine)
    {
        stateMachine = monsterStateMachine;
        groundData = stateMachine.Monster.Data.GroundData;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void HandleInput()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        if (GameManager.Instance.playerDie || GameManager.Instance.NowPlayCutScene)
        {
            if (!stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = true;
            
            return;
        }
        else 
        { 
            if (stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = false;
        }

        SearchTarget();
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }

    // 애니메이션 종료
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
    }


    private void SearchTarget()
    {
        if (stateMachine.IsAttack) return;
        //Debug.Log("SearchTarget");

        stateMachine.BiggestNoise = 0f;
        Vector3 tempPosition = Vector3.zero;
        stateMachine.Monster.noiseMakers.Clear();
       
        Collider[] temp = Physics.OverlapSphere(stateMachine.Monster.transform.position, stateMachine.Monster.Data.GroundData.PlayerChasingRange * 2, stateMachine.Monster.targetLayer);

        foreach (Collider col in temp)
        {        
            if (col.tag == "NoiseMaker" || col.tag == "Player")
            {
                stateMachine.Monster.noiseMakers.Add(col);
                //CheckNoise(col.gameObject.GetComponent<INoise>().CurNoiseAmount);

                if (CheckNoise(col.gameObject.GetComponent<INoise>().CurNoiseAmount))
                {
                    //stateMachine.CurDestination = col.gameObject.transform.position;
                    if(stateMachine.BiggestNoise >= stateMachine.BeforeNoise) tempPosition = col.transform.position;
                }
            }
        }

        if (tempPosition == Vector3.zero) return;

        if (stateMachine.IsFocusNoise || stateMachine.IsChasing)
        {
            // 추적
            if (Vector3.Distance(stateMachine.Monster.transform.position, tempPosition) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange && stateMachine.BiggestNoise >= 5.5f)
            {
                //Debug.Log("집중 추적 - 걷기 감지");
                stateMachine.CurDestination = tempPosition;
                stateMachine.BeforeNoise = stateMachine.BiggestNoise;
                stateMachine.ChangeState(stateMachine.ChaseState);
                return;
            }
        }
        else 
        {
            // 이동
            if (Vector3.Distance(stateMachine.Monster.transform.position, tempPosition) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange && stateMachine.BiggestNoise >= stateMachine.Monster.Data.GroundData.DetectNoiseMax)
            {
                //Debug.Log("기본 이동 - 달리기 감지");
                stateMachine.CurDestination = tempPosition;
                stateMachine.BeforeNoise = stateMachine.BiggestNoise;
                stateMachine.ChangeState(stateMachine.MoveState);
                return;
            }

            if (Vector3.Distance(stateMachine.Monster.transform.position, tempPosition) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange * 0.5f && stateMachine.BiggestNoise >= stateMachine.Monster.Data.GroundData.DetectNoiseMid)
            {
                //Debug.Log("기본 이동 - 걷기 감지");
                stateMachine.CurDestination = tempPosition;
                stateMachine.BeforeNoise = stateMachine.BiggestNoise;
                stateMachine.ChangeState(stateMachine.MoveState);
                return;
            }
        }

        //// 아이템, 장비 소음 발생***
        //if (stateMachine.BiggestNoise >= 90f)
        //{
        //    //Debug.Log("아이템 감지");
        //    stateMachine.CurDestination = tempPosition;
        //    stateMachine.ChangeState(stateMachine.MoveState);
        //    return;
        //}

        //if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange && stateMachine.BiggestNoise >= 11.5f)
        //{
        //    //Debug.Log("달리기 감지");
        //    stateMachine.CurDestination = tempPosition;
        //    stateMachine.ChangeState(stateMachine.MoveState);
        //    return;
        //}

        //if (Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position) <= stateMachine.Monster.Data.GroundData.PlayerChasingRange * 0.5f && stateMachine.BiggestNoise >= 5.5f)
        //{
        //    //Debug.Log("걷기 감지");
        //    stateMachine.CurDestination = tempPosition;
        //    stateMachine.ChangeState(stateMachine.MoveState);
        //    return;
        //}
    }

    protected bool CheckNoise(float curNoise)
    {
        if (stateMachine.BiggestNoise < curNoise)
        {
            stateMachine.BiggestNoise = curNoise;
            return true;
        }
        else return false;
    }

    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= groundData.AttackRange * groundData.AttackRange;
    }

    protected Vector3 GetMovementDirection()
    {
        if (stateMachine.IsMove)
        {
            Vector3 noiseDir = (stateMachine.CurDestination - stateMachine.Monster.transform.position).normalized;
            return noiseDir;
        }

        if (stateMachine.IsComeBack)
        {
            Vector3 backDir = (stateMachine.StartPosition - stateMachine.Monster.transform.position).normalized;
            return backDir;
        }       

        Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).normalized;

        return dir;
    }

    protected void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            stateMachine.Monster.transform.rotation = Quaternion.Lerp(stateMachine.Monster.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void MoveToPosition(Vector3 postion, float distance)
    {
        // 목표 지점 근처의 유효한 네비게이션 메쉬 위치 찾기
        if (NavMesh.SamplePosition(postion, out NavMeshHit hit, distance, NavMesh.AllAreas))
        {
            stateMachine.Monster.Agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError("이동 불가 지역");
        }
    }


}
