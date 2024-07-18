using UnityEngine;
using UnityEngine.UI;
using System;

public class DayNightUI : MonoBehaviour
{
    public RawImage dayImage;
    public Texture2D dayTexture;
    public Texture2D nightTexture;

    void Update()
    {
       
        float currentTime = DateTime.Now.Hour; 

        if (currentTime >= 6 && currentTime < 18)
        {
            // �� �ð���
            dayImage.texture = dayTexture;
        }
        else
        {
            // �� �ð���
            dayImage.texture = nightTexture;
        }
    }
}