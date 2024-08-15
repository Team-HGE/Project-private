using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupTypeMonster : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterGroupTypeAnimationData AnimationData { get; private set; }

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterGroupTypeStateMachine _stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        //Ai Nav
        Agent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();

        _stateMachine = new MonsterGroupTypeStateMachine(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.ChaseState);
    }

    private void Update()
    {
        _stateMachine.Update();
    }


    //private void FixedUpdate()
    //{
    //    _stateMachine.PhysicsUpdate();
    //}

}
