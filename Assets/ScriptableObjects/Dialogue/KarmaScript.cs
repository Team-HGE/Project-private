using System.Collections;
using UnityEngine;

public class KarmaScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;

    public void Init(ScriptSO _script)
    {
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ���� 
        if (isTalking) return;
        isTalking = true;

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < 1; i++)
        {
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[RandomKarmaIndex()]);
            ui.SetImage(ui.portrait, scriptSO.images[RandomKarmaIndex()]);
            ui.CheckNullTitle(scriptSO.speakers[RandomKarmaIndex()]);

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[RandomKarmaIndex()]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("��Ŭ������ �����ϼ���");
            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        yield return null;
    }

    private int RandomKarmaIndex()
    {
        System.Random rng = new System.Random();
        int ranIndex = -1;

        //���� ī���� ��ġ Ȯ��
        //ī������ ������ ��, ����� ��, 0�̸� �߸� �޽��� ���

        if (GameManager.Instance.PlayerStateMachine.Player.Karma < 0)
        {
            // ī���� ��
            ranIndex = rng.Next(0, 5);
        }
        else if (GameManager.Instance.PlayerStateMachine.Player.Karma > 0)
        {
            // ī���� ��
            ranIndex = rng.Next(2, 7);
        }
        else
        {
            // ī���� �߸�
            ranIndex = rng.Next(2, 5);
        }

        return ranIndex;
    }
}