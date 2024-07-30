using UnityEngine;
using System.Collections;

public class NPCScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;
    private NPC npc;

    public void Init(ScriptSO _script)
    {
        scriptSO = _script;
        ui.CloseDialogue();
    }

    public void Print()
    {
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ���� 
        if (isTalking) { Debug.Log("������ ��ȭ�� �� �����ϴ�."); return; }
        isTalking = true;
        //StopCoroutine(PrintScript());

        // ��ȣ�ۿ� ���� ������Ʈ �Ǻ�
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        npc = nowInteracting.GetComponent<NPC>();

        // npc �� �ƴ� ���
        if (npc == null) { Debug.Log("NPC�� �ƴմϴ�. �Ǵ� NPC ������Ʈ�� �����ϴ�."); return; }

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
        //isTalking = false;
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            // ���ϴ� NPC �̸� - ��ȭ��
            if (scriptSO.speakers[i] != "")
                UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + npc.ChangeNpcState(NpcState.Speaking));

            ui.SetImage(ui.portrait, scriptSO.images[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            if (scriptSO.bodyTexts[i] == "PickAnswer")
            {
                //Debug.Log("��� �����ϰ� ������ ����մϴ�.");

                UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.Print();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
                continue;
            }

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("��Ŭ������ �����ϼ���");
            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        // NPC ���� ���� ����
        npc.ChangeNpcState(NpcState.Idle);

        yield return null;
    }
}