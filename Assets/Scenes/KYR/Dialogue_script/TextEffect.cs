

using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class TextEffect
{
    // TODO: 텍스트 굵게, 노랑 or 빨갛게 강조, 타이핑 효과, 페이드아웃 효과

    private static float typingPerSeconds = 20f;

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
}