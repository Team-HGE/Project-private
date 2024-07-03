using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Characters/Monster")]

public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public MonsterGroundData GroundData { get; private set; }
}
