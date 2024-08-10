using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventManager eventManager = (EventManager)target;

        // ����ġ ����Ʈ�� ũ�⸦ Enum�� ũ�⿡ ���߱�
        int switchCount = System.Enum.GetValues(typeof(GameSwitch)).Length;
        if (eventManager.switchStates == null || eventManager.switchStates.Count != switchCount)
        {
            eventManager.switchStates = new List<bool>(new bool[switchCount]);
        }

        serializedObject.Update();

        // ��� GameSwitch enum ���� ��ȸ�ϸ鼭 �ν����Ϳ� ǥ��
        for (int i = 0; i < switchCount; i++)
        {
            GameSwitch switchType = (GameSwitch)i;
            eventManager.switchStates[i] = EditorGUILayout.Toggle(switchType.ToString(), eventManager.switchStates[i]);
        }

        serializedObject.ApplyModifiedProperties();
    }
}