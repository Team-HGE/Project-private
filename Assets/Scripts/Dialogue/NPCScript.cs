using UnityEngine;
using System.Collections;
using System.Text;

public class NPCScript : DialogueSetting, IScript
{
    private NPC_SO npcSO;
    private ScriptSO scriptSO;

    public void InitNPC(NpcData data, int ID)
    {
        npcSO = data.NpcList[ID];
        Init(data.LoadNpcSO(ID));

        // NPC ���� ��ȭ������ ����, ��Ʈ���� ���� ����
        data.ChangeNpcState(ID, NpcState.Speaking);
        data.StressDown(ID, 10);
    }

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


        // ��ȣ�ۿ� ���� ������Ʈ �Ǻ�
        //GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        //npc = nowInteracting.GetComponent<NPC>();

        // npc �� �ƴ� ���
        //if (npc == null) { Debug.Log("NPC�� �ƴմϴ�. �Ǵ� NPC ������Ʈ�� �����ϴ�."); return; }

        // NPC ���� ���� �ʱ�ȭ
        //npc.ChangeNpcState(NpcState.Idle);

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("�������� �ڷ�ƾ�� �����մϴ�."); StopAllCoroutines(); InitDialogueSetting(); break; }

            // ���ϴ� NPC �̸� - ��ȭ��
            if (scriptSO.speakers[i] != "")
                UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + npcSO.state);

            ui.SetPortrait(ui.portrait, scriptSO.portraits[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        //�ش� NPC ��ȭ ��ȸ �Ҹ�
        //NPC ���� ��������� ����
        npcSO.hadInteract = true;
        npcSO.state = NpcState.Idle;

        yield return null;
    }
}