using UnityEngine;

public class TranshCanObject : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] BoxCollider _collider;
    public float forceMagnitude = 1000f; // 오브젝트에 가할 힘의 크기
     public float launchAngle = 35f; // 발사 각도

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider collider)
    {
        // 충돌한 오브젝트가 Rigidbody를 가지고 있는지 확인
        Rigidbody rb = collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            _collider.isTrigger = false;
            _rb.isKinematic = false;
            Vector3 forceDirection = CalculateLaunchDirection();
            _rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }

    Vector3 CalculateLaunchDirection()
    {
        // 앞방향에서 15도 위로 회전된 방향 계산
        Vector3 forward = transform.forward;
        Quaternion rotation = Quaternion.AngleAxis(launchAngle, transform.right);
        Vector3 launchDirection = rotation * forward;
        return launchDirection.normalized;
    }
}