using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // ����ġ ���¸� ������ List<bool>
    public List<bool> switchStates;
    public Switch[] switches;

    public struct Switch
    {
        public GameSwitch switchType;
        public bool state;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSwitches();
        }
        else
        {
            Destroy(gameObject);
        }
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