using UnityEngine;
using UnityEngine.UI;
using System;

public class DayNightUI : MonoBehaviour
{
    public RawImage dayImage;
    public Texture2D dayTexture;
    public Texture2D nightTexture;

    void Start()
    {
        // �ؽ�Ʈ �⺻����
        UpdateDayNightUI(EventManager.Instance.GetSwitch(GameSwitch.IsDaytime));

        // �̺�Ʈ ������
        EventManager.Instance.OnSwitchChanged += OnSwitchChanged;
        EventManager.Instance.SetSwitch(GameSwitch.IsDaytime,true);
    }

    void OnDestroy()
    {
        // OnDestroy���� �̺�Ʈ ���� ������
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

    private void UpdateDayNightUI(bool isDaytime)
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