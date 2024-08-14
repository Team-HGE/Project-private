using Cinemachine;
using UnityEngine;

public class PlayerLookRotation : MonoBehaviour
{
    public CinemachineVirtualCamera playerVC;
    //public CinemachineVirtualCamera pauseVC;

    private CinemachinePOV _pov;
    [SerializeField] Transform lookPointTr;
    [SerializeField] Transform offLookPointTr;

    private Player _player;

    private void Start()
    {
        _pov = playerVC.GetCinemachineComponent<CinemachinePOV>();
        _player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        if (_player.IsPauseVC)
        {
            //if (!pauseVC.enabled) pauseVC.enabled = true;
            offLookPointTr.rotation = lookPointTr.rotation;
            return;
        }
        //else
        //{
        //    //if (pauseVC.enabled) pauseVC.enabled = false;
        //}

        lookPointTr.rotation = Quaternion.Euler(_pov.m_VerticalAxis.Value, 0, 0);
        transform.rotation = Quaternion.Euler(0, _pov.m_HorizontalAxis.Value, 0);
    }
}
