using UnityEngine;

public class TranshCanObject : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] BoxCollider _collider;
    public float forceMagnitude = 1000f; // ������Ʈ�� ���� ���� ũ��
     public float launchAngle = 35f; // �߻� ����

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }
    void OnTriggerEnter(Collider collider)
    {
        // �浹�� ������Ʈ�� Rigidbody�� ������ �ִ��� Ȯ��
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
        // �չ��⿡�� 15�� ���� ȸ���� ���� ���
        Vector3 forward = transform.forward;
        Quaternion rotation = Quaternion.AngleAxis(launchAngle, transform.right);
        Vector3 launchDirection = rotation * forward;
        return launchDirection.normalized;
    }
}