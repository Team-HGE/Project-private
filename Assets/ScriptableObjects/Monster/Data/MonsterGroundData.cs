using System;
using UnityEngine;

[Serializable]
public class MonsterGroundData
{
    [field: Header("BaseData")]
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;
    [field: SerializeField][field: Range(0f, 30f)] public float PlayerChasingRange { get; private set; } = 10f;
    [field: SerializeField][field: Range(0f, 10f)] public float AttackRange { get; private set; } = 1.5f;
    [field: SerializeField][field: Range(0f, 180f)] public float ViewAngle { get; private set; } = 120f;


    [field: Header("IdleData")]
    [field: SerializeField][field: Range(0f, 5f)] public float IdleTransitionTime { get; private set; } = 2.5f;



    [field: Header("PatrolData")]
    [field: SerializeField][field: Range(0f, 10f)] public float PatrolSpeed { get; private set; } = 4.5f;
    [field: SerializeField][field: Range(0f, 60f)] public float PatrolRange { get; private set; } = 30f;
    [field: SerializeField] public float PatrolMinDistance { get; private set; }

    [field: Header("MoveData")]
    [field: SerializeField][field: Range(0f, 10f)] public float MoveSpeed { get; private set; } = 4.5f;




    [field: Header("FindData")]
    [field: SerializeField][field: Range(0f, 5f)] public float FindTransitionTime { get; private set; } = 1.5f;

    [field: Header("ChaseData")]
    [field: SerializeField][field: Range(0f, 30f)] public float ChaseSpeed { get; private set; } = 12f;

    [field: Header("LoseSightData")]
    [field: SerializeField][field: Range(0f, 5f)] public float LoseSightTransitionTime { get; private set; } = 1.5f;
}
