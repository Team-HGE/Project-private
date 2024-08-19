using TMPro;
using UnityEngine;

public class FinishBtnUI : MonoBehaviour
{
    public TextMeshProUGUI finishBtnTxt;

    // 버튼을 누를 때 호출되는 메서드
    public void skipon()
    {
        finishBtnTxt.text = "스토리 건너뛰기▶▶";
        //보류중...

    }

}