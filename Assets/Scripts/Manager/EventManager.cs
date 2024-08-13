using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonManager<EventManager>
{

    // ����ġ ���¸� ������ List<bool>
    public List<bool> switchStates;
    public Switch[] switches;
    public event Action<GameSwitch, bool> OnSwitchChanged;

    public struct Switch
    {
        public GameSwitch switchType;
        public bool state;
    }


    private void InitializeSwitches()
    {
        // GameSwitch �������� �׸� ����ŭ List<bool> ũ�⸦ �ʱ�ȭ
        int switchCount = System.Enum.GetValues(typeof(GameSwitch)).Length;
        if (switchStates == null || switchStates.Count != switchCount)
        {
            switchStates = new List<bool>(new bool[switchCount]);
        }
    }

    public void SetSwitch(GameSwitch switchType, bool state)
    {
        // ���� switchStates ����Ʈ���� �ش� ����ġ�� ���¸� ������Ʈ
        int index = (int)switchType;
        if (index >= 0 && index < switchStates.Count)
        {
            switchStates[index] = state; // ���� ����ġ ���� ������Ʈ

            OnSwitchChanged?.Invoke(switchType, state);
        }
        else
        {
            Debug.LogError("Switch index out of bounds!");
            return;
        }

        // switches �迭���� ����ġ�� �̹� �����ϴ��� Ȯ��
       
    }

    public bool GetSwitch(GameSwitch switchType)
    {
        int index = (int)switchType;
        if (index >= 0 && index < switchStates.Count)
        {
            return switchStates[index];
        }
        return false;
    }

    //EventManager.Instance.SetSwitch(GameSwitch.HasKey, true);
}