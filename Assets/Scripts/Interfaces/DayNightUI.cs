using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DayNightUI : MonoBehaviour
{
    [TitleGroup("DayNightUI", "MonoBehaviour", alignment: TitleAlignments.Centered, horizontalLine: true, boldTitle: true, indent: false)]
    [SerializeField] bool toggle;

    [TabGroup("Tab", "Day", SdfIconType.FolderFill, TextColor = "orange")]
    [TabGroup("Tab", "Day")] public RawImage dayImage;
    [TabGroup("Tab", "Day")] public Texture2D dayTexture;
    [TabGroup("Tab", "Day")] public Texture2D nightTexture;

    void Start()
    {
        // 이벤트 구독함
        EventManager.Instance.OnSwitchChanged += OnSwitchChanged;
    }

    void OnDestroy()
    {
        // OnDestroy에서 이벤트 구독 해제함
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwitchChanged -= OnSwitchChanged;
        }
    }

    private void OnSwitchChanged(GameSwitch switchType, bool state)
    {
        if (switchType == GameSwitch.IsDaytime)
        {
            UpdateDayNightUI(state);
        }
    }

    public void UpdateDayNightUI(bool isDaytime)
    {
        if (isDaytime && dayImage.texture != dayTexture)
        {
            dayImage.texture = dayTexture;
        }
        else if (!isDaytime && dayImage.texture != nightTexture)
        {
            dayImage.texture = nightTexture;
        }
    }

}