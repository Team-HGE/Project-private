using UnityEngine;

public class Day1DoorEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] string targetTag = "Lockable";
    [SerializeField] string targetName = "01_low";

    void Start()
    { }

    public void day1Look()
    {
        // ������ �±׸� ���� ��� �� ������Ʈ�� ã���ϴ�.
        GameObject[] doors = GameObject.FindGameObjectsWithTag(targetTag);

        // ��� �� ������Ʈ�� ��ȸ�ϸ� �̸��� ������ ���� ��޴ϴ�.
        foreach (GameObject door in doors)
        {
            // �� ������Ʈ�� �̸��� targetName�� ��ġ�ϴ��� Ȯ���մϴ�.
            if (door.name == targetName)
            {
                DoorObject doorObject = door.GetComponent<DoorObject>();
                if (doorObject != null)
                {
                    doorObject.isLock = true;
                }
            }
        }
    }
}