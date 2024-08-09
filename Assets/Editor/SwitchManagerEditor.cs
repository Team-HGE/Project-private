using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventManager eventManager = (EventManager)target;

        // 스위치 리스트의 크기를 Enum의 크기에 맞추기
        int switchCount = System.Enum.GetValues(typeof(GameSwitch)).Length;
        if (eventManager.switchStates == null || eventManager.switchStates.Count != switchCount)
        {
            eventManager.switchStates = new List<bool>(new bool[switchCount]);
        }

        serializedObject.Update();

        // 모든 GameSwitch enum 값을 순회하면서 인스펙터에 표시
        for (int i = 0; i < switchCount; i++)
        {
            GameSwitch switchType = (GameSwitch)i;
            eventManager.switchStates[i] = EditorGUILayout.Toggle(switchType.ToString(), eventManager.switchStates[i]);
        }

        serializedObject.ApplyModifiedProperties();
    }
}