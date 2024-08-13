using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonManager<EventManager>
{

    // 스위치 상태를 관리할 List<bool>
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
        // GameSwitch 열거형의 항목 수만큼 List<bool> 크기를 초기화
        int switchCount = System.Enum.GetValues(typeof(GameSwitch)).Length;
        if (switchStates == null || switchStates.Count != switchCount)
        {
            switchStates = new List<bool>(new bool[switchCount]);
        }
    }

    public void SetSwitch(GameSwitch switchType, bool state)
    {
        // 먼저 switchStates 리스트에서 해당 스위치의 상태를 업데이트
        int index = (int)switchType;
        if (index >= 0 && index < switchStates.Count)
        {
            switchStates[index] = state; // 기존 스위치 상태 업데이트

            OnSwitchChanged?.Invoke(switchType, state);
        }
        else
        {
            Debug.LogError("Switch index out of bounds!");
            return;
        }

        // switches 배열에서 스위치가 이미 존재하는지 확인
       
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