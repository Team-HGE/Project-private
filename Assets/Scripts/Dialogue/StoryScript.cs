using System.Collections;
using UnityEngine;
using static AudioManager;

public class StoryScript: DialogueSetting, IScript
{
    [HideInInspector]
    public ScriptSO scriptSO; // 추후 private로 수정예정
    public AudioManager audioManager; // 권용 수정 오디오 매니저 참고
    public GameObject waitIcon; // 기다리는 아이콘 참조
    public Quest quest;
    public SystemMsg systemMsg;
    public void Init(ScriptSO _script)
    {
        audioManager = GetComponent<AudioManager>();
        quest = GetComponent<Quest>();
        systemMsg = GetComponent<SystemMsg>();
        //sbTitle = new StringBuilder();
        //sbBody = new StringBuilder();
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
        ui.ObjectPoolInit();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음, 대화 내용 초기화
        if (isTalking) { Debug.Log("지금은 대화할 수 없습니다."); InitDialogueSetting(); return; }
        isTalking = true;

        //Init(scriptSO);
        if (scriptSO == null) { Debug.Log("지금은 내보낼 스크립트가 없습니다. scriptSO null"); return; };

        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        StopAllCoroutines();
        ui.OpenBG();
        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        if (!isTalking)
        {
            Debug.Log("실행중인 코루틴을 종료합니다.");
            InitDialogueSetting();
            yield break;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui.ClearDialogue(sbTitle, sbBody);

        if (scriptSO == null)
        {
            Debug.Log("scriptSO가 음슴");
            InitDialogueSetting();
            yield break;
        }

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking)
            {
                Debug.Log("실행중인 코루틴을 종료합니다.");
                InitDialogueSetting();
                yield break;
            }

            UpdateUI(scriptSO.speakers[i], scriptSO.portraits[i]);

            if (scriptSO.audioClips != null && scriptSO.audioClips[i] != null)
            {
                AudioManager.Instance.PlayDialSE(scriptSO.audioClips[i]); // 권용 오디오 클립 재생 버그해결완
            }

            // 기능 분기
            switch (scriptSO.bodyTexts[i])
            {
                case "PickAnswer":
                    yield return HandlePickAnswer(i);
                    continue;

                case "TryQuest":
                    yield return HandleTryQuest(i);
                    continue;

                case var text when text.StartsWith("NewQuest"):
                    yield return HandleNewQuest(text);
                    continue;


                case "PlayerControl":
                    HandlePlayerControl();
                    continue;

                case var text when text.StartsWith("NewTip"):
                    yield return Tips(text);
                    continue;

                default:
                    curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
                    break;
            }

            yield return StartCoroutine(curPrintLine);
            yield return HandleWaitAndSound(i);

            ui.ClearDialogue(sbTitle, sbBody);
        }

        EndDialogue();
        yield return null;
    }

    private void UpdateUI(string speaker, Sprite portrait)
    {
        UtilSB.SetText(ui.titleText, sbTitle, speaker);
        ui.SetPortrait(ui.portrait, portrait);
        //ui.PopStanding(portrait);
        ui.CheckNullTitle(speaker);
    }

    private IEnumerator HandlePickAnswer(int index)
    {
        UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[index - 1]);
        DialogueManager.Instance.answer.Print();
        yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);
        DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
    }

    private IEnumerator HandleTryQuest(int index)
    {
        Debug.Log("잠깐 스토리 진행을 멈추고 퀘스트가 완료될 때까지 기다립니다.");
        UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[index - 1]);
        quest.TryNextQuest();

        Debug.Log("퀘스트 완료 대기 중입니다.");
        // yield return new WaitUntil(() => 퀘스트가 완료됐을 때의 조건식을 넣어주세요;

        Debug.Log("퀘스트 완료. 다시 스토리를 진행합니다.");
        yield break;
    }

    private IEnumerator HandleNewQuest(string questText)
    {
        Debug.Log("다음 스토리를 갱신합니다.");
        if (questText.StartsWith("NewQuest"))
        {
            string questString = questText.Substring(8); 
            if (int.TryParse(questString, out int questNumber))
            {
                quest.NextQuest(questNumber);
            }

            yield break;
        }
    }

    private void HandlePlayerControl()
    {
        ui.darkScreen.SetActive(false);
        Debug.Log("플레이어 이동 OnOff");
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
    }

    private IEnumerator Tips(string TipText)
    {
        Debug.Log("선택한 팁 메세지를 호출합니다.");
        if (TipText.StartsWith("NewTip"))
        {
            string TipString = TipText.Substring(6);
            if (int.TryParse(TipString, out int TipsNumber))
            {
                systemMsg.UpdateTipMessage(TipsNumber);
            }
        }
            
       
        yield break;
    }

    private IEnumerator HandleWaitAndSound(int index)
    {
        waitIcon.SetActive(true);
        yield return waitLeftClick;
        AudioManager.Instance.PlaySoundEffect(SoundEffect.DialClick);
        //AudioManager.Instance.StopDialSE(scriptSO.audioClips[index]);
        yield return waitTime;
        waitIcon.SetActive(false);
    }

    private void EndDialogue()
    {
        ui.CloseDialogue();
        isTalking = false;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
    }
}