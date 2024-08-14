using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public GameObject playerVC;
    public Light flashLight;

    private void Start()
    {
        audioSource = playerVC.GetComponentInChildren<AudioSource>();
    }
    public void ToggleFlashLight()
    {
        audioSource.Play();
        flashLight.enabled = !flashLight.enabled;
    }
}
