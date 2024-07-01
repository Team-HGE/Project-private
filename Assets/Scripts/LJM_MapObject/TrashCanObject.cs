using UnityEngine;
using DG.Tweening;

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
        //transform.DOShakePosition(5, 3).SetEase(Ease.InOutBounce).onComplete += ;
        transform.DOKill();
    }
}