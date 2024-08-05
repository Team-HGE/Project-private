using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class SystemMsg : MonoBehaviour
{
    public SystemMsgSO systemMsgSO;
    public GameObject systemMsgCanvas;

    private ObjectPool objectPool;
    private StringBuilder sb = new StringBuilder();
    private GameObject msgPrefab;
    private TextMeshProUGUI msgText;

    private bool isUpdating = false;

    public void Init()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    public void UpdateMessage()
    {
        if (isUpdating) return;
        isUpdating = true;

        StartCoroutine(PopMessage());
    }

    private IEnumerator PopMessage()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < systemMsgSO.messages.Length; i++)
        {
            msgPrefab = objectPool.GetObject();
            msgText = msgPrefab.GetComponent<TextMeshProUGUI>();

            sb.Append(systemMsgSO.messages[i]);
            TextEffect.Highlight(msgText, Color.red);
            msgText.text = "SYSTEM: " + sb.ToString();

            StartCoroutine(TextEffect.FadeOut(msgText));
            sb.Clear();

            //Debug.Log(msgPrefab);

            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < systemMsgSO.tips.Length; i++)
        {
            msgPrefab = objectPool.GetObject();
            msgText = msgPrefab.GetComponent<TextMeshProUGUI>();

            sb.Append(systemMsgSO.tips[i]);
            //TextEffect.Highlight(msgText, Color.red);
            msgText.text = "TIP: " + sb.ToString();

            StartCoroutine(TextEffect.FadeOut(msgText));
            sb.Clear();

            //Debug.Log(msgPrefab);

            yield return new WaitForSeconds(1f);
        }

        isUpdating = false;
        yield return null;
    }
}