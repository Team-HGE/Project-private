using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using static AudioManager;

public class SystemMsg : MonoBehaviour
{
    public SystemMsgSO systemMsgSO;
    public GameObject systemMsgCanvas;

    private ObjectPool objectPool;
    private StringBuilder sb = new StringBuilder();
    private GameObject msgPrefab;
    private TextMeshProUGUI msgText;
    public AudioManager audioManager;
    public int NowSystemMsgNumber;
    public int NowTipMsgNumber;
    private bool isUpdating = false;

    public void Init()
    {
        objectPool = GetComponent<ObjectPool>();
        audioManager = GetComponent<AudioManager>();
        NowSystemMsgNumber = 0;
        NowTipMsgNumber = 0;
    }

    public void UpdateMessage(int SystemMsgNumber)
    {
        if (isUpdating) return;
        isUpdating = true;
        NowSystemMsgNumber = SystemMsgNumber;
        StartCoroutine(PopMessage());
    }

    public void UpdateTipMessage(int TipMsgNumber)
    {
        if (isUpdating) return;
        isUpdating = true;
        NowTipMsgNumber = TipMsgNumber;
        StartCoroutine(PopTipMessage());
    }

    public IEnumerator PopMessage()
    {
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySoundEffect(SoundEffect.systemMsg);
        msgPrefab = objectPool.GetObject();
        msgText = msgPrefab.GetComponent<TextMeshProUGUI>();
        sb.Append(systemMsgSO.messages[NowSystemMsgNumber]);
        TextEffect.Highlight(msgText, Color.red);
        msgText.text = sb.ToString();

        StartCoroutine(TextEffect.FadeOut(msgText));
        sb.Clear();

        //Debug.Log(msgPrefab);

        yield return new WaitForSeconds(1f);


        //    private IEnumerator PopMessage()
        //{
        //    yield return new WaitForSeconds(1f);

        //    for (int i = 0; i < systemMsgSO.messages.Length; i++)
        //    {
        //        msgPrefab = objectPool.GetObject();
        //        msgText = msgPrefab.GetComponent<TextMeshProUGUI>();

        //        sb.Append(systemMsgSO.messages[i]);
        //        TextEffect.Highlight(msgText, Color.red);
        //        msgText.text = "SYSTEM: " + sb.ToString();

        //        StartCoroutine(TextEffect.FadeOut(msgText));
        //        sb.Clear();

        //        //Debug.Log(msgPrefab);

        //        yield return new WaitForSeconds(1f);
        //    }

        //    for (int i = 0; i < systemMsgSO.tips.Length; i++)
        //    {
        //        msgPrefab = objectPool.GetObject();
        //        msgText = msgPrefab.GetComponent<TextMeshProUGUI>();

        //        sb.Append(systemMsgSO.tips[i]);
        //        //TextEffect.Highlight(msgText, Color.red);
        //        msgText.text = "TIP: " + sb.ToString();

        //        StartCoroutine(TextEffect.FadeOut(msgText));
        //        sb.Clear();

        //        //Debug.Log(msgPrefab);

        //        yield return new WaitForSeconds(1f);
        //    }

        isUpdating = false;
        yield return null;
    }

    public IEnumerator PopTipMessage()
    {
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySoundEffect(SoundEffect.systemMsg);
        msgPrefab = objectPool.GetObject();
        msgText = msgPrefab.GetComponent<TextMeshProUGUI>();
        sb.Append(systemMsgSO.tips[NowTipMsgNumber]);
        TextEffect.Highlight(msgText, Color.white);
        msgText.text = sb.ToString();

        StartCoroutine(TextEffect.FadeOut(msgText));
        sb.Clear();

        //Debug.Log(msgPrefab);

        yield return new WaitForSeconds(1f);

        isUpdating = false;
        yield return null;
    }
}