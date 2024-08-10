using System.Collections;
using UnityEngine;
using static AudioManager;

public class StoryScript: DialogueSetting, IScript
{
    [HideInInspector]
    public ScriptSO scriptSO; // 추후 private로 수정예정
    public AudioManager audioManager; // 권용 수정 오디오 매니저 참고
    public GameObject waitIcon; // 기다리는 아이콘 참조

    public void Init(ScriptSO _script)
    {
        //sbTitle = new StringBuilder();
        //sbBody = new StringBuilder();
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음, 대화 내용 초기화
        if (isTalking) { Debug.Log("지금은 대화할 수 없습니다."); InitDialogueSetting(); return; }
        isTalking = true;

        //Init(scriptSO);
        if (scriptSO == null) { Debug.Log("지금은 내보낼 스크립트가 없습니다. scriptSO null"); return; };

        //GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();//임시 주석 처리 - 다이얼로그 끝난 뒤 플래이어 정지 해제 안됨***
        StopAllCoroutines();
        ui.OpenBG();
        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        //if (!isTalking) { Debug.Log("실행중인 코루틴을 종료합니다."); StopAllCoroutines(); InitDialogueSetting(); }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui.ClearDialogue(sbTitle, sbBody);

        if (scriptSO == null) { Debug.Log("scriptSO null"); StopAllCoroutines(); InitDialogueSetting(); yield break; };

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("실행중인 코루틴을 종료합니다."); StopAllCoroutines(); InitDialogueSetting(); break; }

            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i]);

            ui.SetImage(ui.portrait, scriptSO.images[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            if (scriptSO.audioClips != null)
            {
                AudioManager.Instance.PlayDialSE(scriptSO.audioClips[i]); // 권용 오디오 클립 재생
            }


            if (scriptSO.bodyTexts[i] == "PickAnswer")
            {
                //Debug.Log("잠깐 정지하고 선택지 출력합니다.");

                UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.Print();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
                continue;
            }
            else if (scriptSO.bodyTexts[i] == "CheckQuest") // scriptSO 출력 중 CheckQuest 문구가 나올 때
            {
                Debug.Log("잠깐 스토리 진행을 멈추고 퀘스트가 완료될 때까지 기다립니다.");

                UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]); // 이전 스크립트를 띄워둡니다.

                Debug.Log("퀘스트 완료 대기 중입니다.");
                // yield return new WaitUntil(() => 퀘스트가 완료됐을 때의 조건식을 넣어주세요;

                //여기서 퀘스트를 갱신하세요

                Debug.Log("퀘스트 완료. 다시 스토리를 진행합니다.");
                continue;
            }
            else if(scriptSO.bodyTexts[i] == "PlayerControl")  // 다이얼로그 진입 시 플레이어 이동 기본 상태: OFF
            {
                ui.darkScreen.SetActive(false);
                Debug.Log("플레이어 이동 OnOff");
                GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
            }

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            
            yield return StartCoroutine(curPrintLine);

            waitIcon.SetActive(true); //권용 추가 기다리는 아이콘 등장

            //Debug.Log("좌클릭으로 진행하세요");
            yield return waitLeftClick;

            AudioManager.Instance.PlaySoundEffect(SoundEffect.DialClick); // 권용 수정 사운드 재생
            AudioManager.Instance.StopDialSE(scriptSO.audioClips[i]); //권용 수정 사운드 재생 다이얼SE 멈춤
            //Debug.Log("좌클릭으로 진행하세요1");

            yield return waitTime;

            waitIcon.SetActive(false); // 권용 추가 기다리는 아이콘 사라짐
          
            //Debug.Log("좌클릭으로 진행하세요2");

            ui.ClearDialogue(sbTitle, sbBody);
        }

        
        ui.CloseDialogue();
        isTalking = false;

        yield return null;
    }
}