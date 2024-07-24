using UnityEngine;
using UnityEngine.UI;
using System;

public class DayNightUI : MonoBehaviour
{
    public RawImage dayImage;
    public Texture2D dayTexture;
    public Texture2D nightTexture;
    float currentTime = 1;

    void Start()
    {

        dayImage.texture = dayTexture;

    
    }

    public void TimeUpdate()
    {
        currentTime++;
    }

    private void Update()
    {
        if (currentTime == 1)
        {
            // �� �ð���
            dayImage.texture = dayTexture;
        }
        else if (currentTime % 2 == 0)
        {
            // �� �ð���
            dayImage.texture = nightTexture;
        }
        else if (currentTime % 2 == 1)
        {
            // �� �ð���
            dayImage.texture = dayTexture;
        }
        
    }
}