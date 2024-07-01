using Cinemachine;
using UnityEngine;

public class PlayerLookRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV _pov;

    private void Start()
    {
        _pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(_pov.m_VerticalAxis.Value, _pov.m_HorizontalAxis.Value, 0);
    }
}
