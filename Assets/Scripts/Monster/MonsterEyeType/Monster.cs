using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using static UnityEngine.Rendering.HableCurve;

public class Monster : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }

    [field: Header("Behavior")]
    [field: SerializeField] public bool CanPatrol { get; set; } = true;
    [SerializeField][field: Range(0f, 50f)] public float patrolRange;


    [field: Header("MonsterTransform")]
    public Transform monsterTransform;
    public Transform monsterEyeTransform;

    [field: Header("Find")]
    public LayerMask playerMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    public bool canCheck;
    public Transform eye;
    public Transform findTarget;

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterStateMachine _stateMachine;

    public bool IsBehavior { get; set; } = true;
    private Coroutine _wait;
    private bool _isWaiting = false;


    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        //Ai Nav
        Agent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();

        _stateMachine = new MonsterStateMachine(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
        StartCoroutine(FPRoutine());
    }

    private void Update()
    {       
        _stateMachine.Update();

        // 범위 체크용, 나중에 주석처리 또는 지울것    
        DrawCircle(transform.position, 36, Data.GroundData.PlayerChasingRange, Color.yellow);
        DrawCircle(transform.position, 36, Data.GroundData.PlayerFindRange, Color.green);
        DrawCircle(transform.position, 36, Data.GroundData.AttackRange, Color.red);
    }

    private IEnumerator FPRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true) 
        {
            yield return wait;
            FindPlayer();
        }            
    }

    private void FindPlayer()
    {
        if (_stateMachine.IsAttack) return; 

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Data.GroundData.PlayerChasingRange, playerMask);

        if (rangeChecks.Length > 0)
        {
            if (!_stateMachine.IsPatrol && !_stateMachine.IsIdle && !_stateMachine.IsComeBack)
            {
                findTarget = rangeChecks[0].transform;
                canCheck = true;
                //if (Vector3.Distance(transform.position, rangeChecks[0].transform.position))
            }
            else if (_stateMachine.IsPatrol || _stateMachine.IsIdle || _stateMachine.IsComeBack)
            {
                if (Vector3.Distance(transform.position, rangeChecks[0].transform.position) <= Data.GroundData.PlayerFindRange)
                {
                    findTarget = rangeChecks[0].transform;
                    canCheck = true;
                }
                else 
                {
                    canCheck = false;
                }
            }
        }
        else
        {
            canCheck = false;
        }
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    //// 대기 시간
    //public void WaitForBehavior(float time)
    //{
    //    StartCoroutine(ChangeBehavior(time));
    //}

    //public IEnumerator ChangeBehavior(float time)
    //{
    //    // n초 대기
    //    yield return new WaitForSeconds(time);
    //    IsBehavior = !IsBehavior;
    //}

    public void WaitForBehavior(float time)
    {
        _wait = StartCoroutine(ChangeBehavior(time));
    }

    public IEnumerator ChangeBehavior(float time)
    {
        _isWaiting = true;
        //_waitTiem = 0f;
        //Debug.Log($"{Data.GroundData.FocusTransitionTime}초 대기");
        // n초 대기
        yield return new WaitForSeconds(time);

        IsBehavior = !IsBehavior;
        //Debug.Log($"{Data.GroundData.FocusTransitionTime}초 대기 끝, {IsBehavior}");
        _isWaiting = false;
    }

    public void StopWait()
    {
        if (!_isWaiting) return;
        StopCoroutine(_wait);
    }

    private void DrawCircle(Vector3 center, int segments, float radius, Color color)
    {
        Vector3 normal = Vector3.up;

        float angleStep = 360.0f / segments;
        Quaternion rotation = Quaternion.LookRotation(normal);  // 법선 벡터를 기준으로 회전

        Vector3 prevPoint = center + rotation * new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Vector3 currentPoint = center + rotation * point;

            Debug.DrawLine(prevPoint, currentPoint, color);  // 이전 점과 현재 점을 연결하여 선을 그림
            prevPoint = currentPoint;
        }

        // 마지막 점과 첫 번째 점을 연결하여 원을 완성
        Vector3 firstPoint = center + rotation * new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);
        Debug.DrawLine(prevPoint, firstPoint, color);
    }
}
