using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Answer", order = 4)]

public class AnswerSO: ScriptableObject
{
    public int nowAnswer;
    public string[] answers;
    // 선택지별 스크립트 추가 
    // 선택지별 플레이어 카르마 스탯 증감치 추가 
}