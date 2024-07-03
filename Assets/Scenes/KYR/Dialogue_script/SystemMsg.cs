using System.Text;
using TMPro;
using UnityEngine;

public class SystemMsg: MonoBehaviour
{
    public GameObject systemMsgCanvas;
    public SystemMsgSO systemMsgSO;

    private ObjectPool objectPool;
    private StringBuilder sb = new StringBuilder();
    private GameObject msgPrefab;
    private TextMeshProUGUI msgText;

    public void Init()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    private void InitSOData(SystemMsgSO _message)
    {
        systemMsgSO = _message;
    }

    public void UpdateMessage()
    {
        msgPrefab = objectPool.GetObject();

        sb.Append(systemMsgSO.messages[0]);
        msgText = msgPrefab.GetComponent<TextMeshProUGUI>();
        msgText.text = sb.ToString();
    }
}