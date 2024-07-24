using System;
using UnityEngine;
[Serializable]
public class MonsterEarTypeAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string patrolParameterName = "Patrol";
    [SerializeField] private string moveParameterName = "Move";

    [SerializeField] private string findParameterName = "Find";
    [SerializeField] private string chaseParameterName = "Chase";
    [SerializeField] private string loseSightParameterName = "LoseSight";

    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int PatrolParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }

    public int FindParameterHash { get; private set; }
    public int ChaseParameterHash { get; private set; }
    public int LoseSightParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        PatrolParameterHash = Animator.StringToHash(patrolParameterName);
        FindParameterHash = Animator.StringToHash(findParameterName);
        ChaseParameterHash = Animator.StringToHash(chaseParameterName);
        LoseSightParameterHash = Animator.StringToHash(loseSightParameterName);
    }
}
