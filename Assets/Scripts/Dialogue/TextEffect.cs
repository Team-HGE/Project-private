using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class TextEffect: DialogueSetting
{
    // TODO: 텍스트 노랑 or 빨갛게 강조, 타이핑 효과, 페이드아웃 효과, 취소선

    public static bool isSkipped { get; set; }
    private static float typingPerSeconds = 20f;
    private static float TextFadeOutSpeed = 1.4f;

    public static IEnumerator Typing(TextMeshProUGUI tmp, StringBuilder sb, string SOstr)
    {
        UtilSB.ClearText(tmp, sb);

        foreach (char c in SOstr.ToCharArray())
        {
            if (!isTalking)
            {
                SOstr = null;
                break;
            }

            CheckSkip();

            if (isSkipped) break;
            
            UtilSB.AppendText(tmp, sb, c);
            yield return new WaitForSeconds(1f / typingPerSeconds);
        }

        if (isSkipped)
        {
            UtilSB.SetText(tmp, sb, SOstr);
            isSkipped = false;
            //Debug.Log("스킵 실행");
        }

        yield return null;
    }
    public static IEnumerator FadeOut(TextMeshProUGUI tmp)
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        while (elapsedTime < TextFadeOutSpeed)
        {
            elapsedTime += Time.deltaTime;
            Color color = tmp.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / TextFadeOutSpeed);
            tmp.color = color;
            yield return null;
        }
        yield return null;
    }

    public static void Highlight(TextMeshProUGUI tmp, Color color)
    {
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = color;
    }

    public static void CorrectLine(TextMeshProUGUI tmp)
    {

    }

    private static void CheckSkip()
    {
        if (!isSkipped && Input.GetMouseButtonDown(0))
        {
            isSkipped = true;
            //Debug.Log("좌클릭 감지, isSkipped:" + isSkipped);
        }
    }
}