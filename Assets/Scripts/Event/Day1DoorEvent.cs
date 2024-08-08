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
        // 지정된 태그를 가진 모든 문 오브젝트를 찾습니다.
        GameObject[] doors = GameObject.FindGameObjectsWithTag(targetTag);

        // 모든 문 오브젝트를 순회하며 이름이 지정된 문만 잠급니다.
        foreach (GameObject door in doors)
        {
            // 문 오브젝트의 이름이 targetName과 일치하는지 확인합니다.
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