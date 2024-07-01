using UnityEngine;
using DG.Tweening;

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
        //transform.DOShakePosition(5, 3).SetEase(Ease.InOutBounce).onComplete += ;
        transform.DOKill();
    }
}