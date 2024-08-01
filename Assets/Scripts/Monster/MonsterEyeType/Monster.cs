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

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterStateMachine _stateMachine;

    // �ൿ ����
    public bool IsBehavior {get; set;} = true;

    [field: SerializeField]
    public bool CanPatrol { get; set; } = true;

    // INoise
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set; }
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    [Header("MonsterTransform")]
    public Transform monsterTransform;
    public Transform monsterEyeTransform;

    [field: Header("Find")]
    public LayerMask playerMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    //public Collider[] rangeChecks;
    public bool canCheck;
    public Transform eye;
    public Transform findTarget;




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
        //_stateMachine.HandleInput();
        _stateMachine.Update();

        //if (NoiseAmount > 0)
        //{
            
        //}

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




        //if (_stateMachine.IsPatrol || _stateMachine.IsIdle)
        //{
        //    rangeChecks = Physics.OverlapSphere(transform.position, Data.GroundData.PlayerFindRange, playerMask);
        //    if (rangeChecks.Length > 0)
        //    {
        //        findTarget = rangeChecks[0].transform;
        //        canCheck = true;
        //    }

        //    //if (rangeChecks.Length > 0) Debug.Log($"�÷��̾� �߰�1, {Data.GroundData.PlayerFindRange}, {Vector3.Distance(transform.position, _stateMachine.Target.transform.position)}");
        //}
        //else
        //{
        //    rangeChecks = Physics.OverlapSphere(transform.position, Data.GroundData.PlayerChasingRange, playerMask);
        //    if (rangeChecks.Length > 0)
        //    {
        //        findTarget = rangeChecks[0].transform;
        //        canCheck = true;
        //    }

        //    //if (rangeChecks.Length > 0) Debug.Log($"�÷��̾� �߰�2, {Data.GroundData.PlayerChasingRange}, {Vector3.Distance(transform.position, _stateMachine.Target.transform.position)}");
        //}
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    // ��� �ð�
    public void WaitForBehavior(float time)
    {
        StartCoroutine(ChangeBehavior(time));
    }

    public IEnumerator ChangeBehavior(float time)
    {
        // n�� ���
        yield return new WaitForSeconds(time);
        IsBehavior = !IsBehavior;
    }

    private void DrawCircle(Vector3 center, int segments, float radius, Color color)
    {
        Vector3 normal = Vector3.up;

        float angleStep = 360.0f / segments;
        Quaternion rotation = Quaternion.LookRotation(normal);  // ���� ���͸� �������� ȸ��

        Vector3 prevPoint = center + rotation * new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Vector3 currentPoint = center + rotation * point;

            Debug.DrawLine(prevPoint, currentPoint, color);  // ���� ���� ���� ���� �����Ͽ� ���� �׸�
            prevPoint = currentPoint;
        }

        // ������ ���� ù ��° ���� �����Ͽ� ���� �ϼ�
        Vector3 firstPoint = center + rotation * new Vector3(Mathf.Cos(0) * radius, Mathf.Sin(0) * radius, 0);
        Debug.DrawLine(prevPoint, firstPoint, color);
    }
}
