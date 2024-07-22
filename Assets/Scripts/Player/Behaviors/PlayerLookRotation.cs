using Cinemachine;
using UnityEngine;

public class PlayerLookRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV _pov;
    [SerializeField] Transform lookPointTr;
    private Player _player;

    private void Start()
    {
        _pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        if (!_player.IsPlayerControll) return;

        lookPointTr.rotation = Quaternion.Euler(_pov.m_VerticalAxis.Value, 0, 0);
        transform.rotation = Quaternion.Euler(0, _pov.m_HorizontalAxis.Value, 0);
    }
}
