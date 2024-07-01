using System.Text;
using TMPro;

public class SBSetting
{
    public static void ClearText(TextMeshProUGUI tmp, StringBuilder sb)
    {
        sb.Clear();
        tmp.text = sb.ToString();
    }

    public static void AppendText(TextMeshProUGUI tmp, StringBuilder sb, string newText)
    {
        sb.Append(newText);
        tmp.text = sb.ToString();
    }
    public static void AppendText(TextMeshProUGUI tmp, StringBuilder sb, char newText)
    {
        sb.Append(newText);
        tmp.text = sb.ToString();
    }

    public static void SetText(TextMeshProUGUI tmp, StringBuilder sb, string newText)
    {
        sb.Clear();
        sb.Append(newText);
        tmp.text = sb.ToString();
    }
    public static void SetText(TextMeshProUGUI tmp, StringBuilder sb, char newText)
    {
        sb.Clear();
        sb.Append(newText);
        tmp.text = sb.ToString();
    }

}