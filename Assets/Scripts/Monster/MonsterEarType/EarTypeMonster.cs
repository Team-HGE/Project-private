using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EarTypeMonster : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterEarTypeAnimationData AnimationData { get; private set; }

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterEarTypeStateMachine _stateMachine;

    // 행동 관리
    public bool IsBehavior { get; set; } = true;

    public LayerMask targetLayer;
    //public Collider[] noiseMakers;
    public List<Collider> noiseMakers;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        //Ai Nav
        Agent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();

        _stateMachine = new MonsterEarTypeStateMachine(this);
        noiseMakers = new List<Collider>();
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.PatrolState);
        //_stateMachine.ChangeState(_stateMachine.MoveState);

    }

    private void Update()
    {
        _stateMachine.Update();

        DrawCircle(transform.position, 36, Data.GroundData.PlayerChasingRange, Color.green);
        DrawCircle(transform.position, 36, 50f, Color.green);

        DrawCircle(transform.position, 36, Data.GroundData.AttackRange, Color.red);

    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

    // 대기 시간
    public void WaitForBehavior(float time)
    {
        StartCoroutine(ChangeBehavior(time));
    }

    public IEnumerator ChangeBehavior(float time)
    {
        // n초 대기
        yield return new WaitForSeconds(time);

        IsBehavior = !IsBehavior;
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
