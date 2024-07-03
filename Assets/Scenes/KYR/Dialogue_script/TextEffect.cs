using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class TextEffect
{
    // TODO: 텍스트 굵게, 노랑 or 빨갛게 강조, 타이핑 효과, 페이드아웃 효과

    private static float typingPerSeconds = 20f;
    private static float TextFadeOutSpeed = 0.5f;

    public static IEnumerator Typing(TextMeshProUGUI tmp, StringBuilder sb, string SOstr)
    {
        UtilSB.ClearText(tmp, sb);

        foreach (char c in SOstr.ToCharArray())
        {
            UtilSB.AppendText(tmp, sb, c);
            yield return new WaitForSeconds(1f / typingPerSeconds);
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
}