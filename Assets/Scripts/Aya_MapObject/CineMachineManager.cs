using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    string VCam = "VCam";
    
    private CinemachineVirtualCamera targetCamera;
    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineBrain mainCamera;
    [SerializeField] CinemachineBlendListCamera blendListCamera;


    Transform playerVCParent;
    Transform targetVCParent;
    public IEnumerator LookTarget(CinemachineVirtualCamera targetCamera)
    {
        if (playerVC == null)
        {
            playerVC = GameObject.FindGameObjectWithTag(VCam).GetComponent<CinemachineVirtualCamera>();
        }
        yield return StartCoroutine(SetBlendCamera(0, targetCamera));
        
    }

    IEnumerator SetBlendCamera(int index, CinemachineVirtualCamera target)
    {
        targetCamera = target;
        if (blendListCamera != null && index >= 0 && index < blendListCamera.m_Instructions.Length)
        {
            playerVCParent = playerVC.transform.parent;
            targetVCParent = targetCamera.transform.parent;
            targetCamera.transform.SetParent(blendListCamera.transform);
            playerVC.transform.SetParent(blendListCamera.transform);
            blendListCamera.m_Instructions[1].m_VirtualCamera = targetCamera;
            blendListCamera.m_Instructions[index].m_VirtualCamera = playerVC;
        }
        float waitTime = 0;
        for (int i = 0; i < 2; i++)
        {
            waitTime += blendListCamera.m_Instructions[i].m_Blend.m_Time;
            waitTime += blendListCamera.m_Instructions[i].m_Hold;
        }
        targetCamera.Priority = 10;
        playerVC.Priority = 1;
        yield return new WaitForSeconds(waitTime);
        playerVC.gameObject.SetActive(false);
    }

    public IEnumerator ReturnToMainCamera()
    {
        playerVC.gameObject.SetActive(true);
        blendListCamera.m_Instructions[0].m_VirtualCamera = targetCamera;
        blendListCamera.m_Instructions[1].m_VirtualCamera = playerVC;
        targetCamera.Priority = 1;
        playerVC.Priority = 10;
        float waitTime = 0;
        for (int i = 0; i < 2; i++)
        {
            waitTime += blendListCamera.m_Instructions[i].m_Blend.m_Time;
            waitTime += blendListCamera.m_Instructions[i].m_Hold;
        }
        yield return new WaitForSeconds(waitTime);
        playerVC.transform.SetParent(playerVCParent);
        targetCamera.transform.SetParent(targetVCParent);
    }
}
