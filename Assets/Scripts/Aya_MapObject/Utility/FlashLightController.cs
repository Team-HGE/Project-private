using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Light FlashlightLight;
    [SerializeField] private GameObject FlashLightAnims;
    [SerializeField] private Animator animator;

    public string FlashlightDrawState = "FlashlightDraw";
    public string FlashlightHideState = "FlashlightHide";
    public string FlashlightReloadState = "FlashlightReload";
    public string FlashlightIdleState = "FlashlightIdle";

    public float FlashlightHideTrim = 0.5f;

    public string FlashlightHideTrigger = "Hide";
    public string FlashlightReloadTrigger = "Reload";

    public AudioClip FlashlightClickOn;
    public AudioClip FlashlightClickOff;

    [SerializeField] private bool flashlight_Enable { get; set; }
    [SerializeField] private bool aniEnd { get; set; }

    private void Start()
    {
        aniEnd = true;
    }

    public void ToggleFlashLight()
    {
        if (aniEnd)
        {
            aniEnd = false;
            if (flashlight_Enable)
            {
                StartCoroutine(HideFlashlight());
                Animator.SetTrigger(FlashlightHideTrigger);
            }
            else
            {
                StartCoroutine(IdleFlashlight());
                Animator.SetTrigger(FlashlightIdleState);
            }
        }
    }

    IEnumerator IdleFlashlight()
    {
        FlashLightAnims.SetActive(true);
        yield return new WaitForAnimatorClip(Animator, FlashlightIdleState);
        SetLightState(true);
        aniEnd = true;
        flashlight_Enable = true;
    }
    IEnumerator HideFlashlight()
    {
        yield return new WaitForAnimatorClip(Animator, FlashlightHideState, FlashlightHideTrim);

        SetLightState(false);
        aniEnd = true;
        flashlight_Enable = false;
        FlashLightAnims.SetActive(false);
    }
    public void SetLightState(bool state)
    {
        FlashlightLight.enabled = state;
        if (!state) audioSource.PlayOneShot(FlashlightClickOff);
        else audioSource.PlayOneShot(FlashlightClickOn);
    }

    public Animator Animator
    {
        get
        {
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            return animator;
        }
    }


    public class WaitForAnimatorClip : CustomYieldInstruction
    {
        const string BaseLayer = "Base Layer";

        private readonly Animator animator;
        private readonly float timeOffset;
        private readonly int stateHash;

        private bool isStateEntered;
        private float stateWaitTime;
        private float timeWaited;

        public WaitForAnimatorClip(Animator animator, string state, float timeOffset = 0, bool normalized = false)
        {
            this.animator = animator;
            this.timeOffset = timeOffset;
            stateHash = Animator.StringToHash(BaseLayer + "." + state);
        }

        public override bool keepWaiting
        {
            get
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

                if (info.fullPathHash == stateHash && !isStateEntered)
                {
                    float stateLength = info.length / info.speed;
                    stateWaitTime = stateLength - timeOffset;
                    isStateEntered = true;
                }
                else if (isStateEntered)
                {
                    if (timeWaited < stateWaitTime)
                        timeWaited += Time.deltaTime;
                    else return false;
                }

                return true;
            }
        }
    }
}
