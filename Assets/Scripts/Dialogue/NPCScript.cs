using UnityEngine;
using System.Collections;
using System.Text;

public class NPCScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;
    private NPC npc;

    public void Init(ScriptSO _script)
    {
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ����, ��ȭ ���� �ʱ�ȭ
        if (isTalking) { Debug.Log("������ ��ȭ�� �� �����ϴ�."); InitDialogueSetting(); return; }
        isTalking = true;

        //�÷��̾� ������ ����

        // ��ȣ�ۿ� ���� ������Ʈ �Ǻ�
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        npc = nowInteracting.GetComponent<NPC>();

        // npc �� �ƴ� ���
        if (npc == null) { Debug.Log("NPC�� �ƴմϴ�. �Ǵ� NPC ������Ʈ�� �����ϴ�."); return; }

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        //if (!isTalking) { Debug.Log("�������� �ڷ�ƾ�� �����մϴ�."); StopAllCoroutines(); InitDialogueSetting();}

        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("�������� �ڷ�ƾ�� �����մϴ�."); StopAllCoroutines(); InitDialogueSetting(); break; }

            // ���ϴ� NPC �̸� - ��ȭ��
            if (scriptSO.speakers[i] != "")
                UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + npc.ChangeNpcState(NpcState.Speaking));

            ui.SetPortrait(ui.portrait, scriptSO.portraits[i]);
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
            //Debug.Log(sbTitle); null �ߵ�
        }

        ui.CloseDialogue();
        isTalking = false;

        //�÷��̾� ������ �簳

        // NPC ���� ���� ����
        npc.ChangeNpcState(NpcState.Idle);

        //�ش� NPC ��ȭ ��ȸ �Ҹ� 
        npc.npcSO.isInteracted = true;

        yield return null;
    }
}