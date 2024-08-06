using System.Collections.Generic;
using UnityEngine;

public class PlayerDangerBeat : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public AudioSource bgmSource; // BGM AudioSource
    public List<Transform> objects; // 특정 오브젝트들의 Transform 리스트
    public float maxVolume = 1.0f; // 최대 볼륨
    public float minVolume = 0.0f; // 최소 볼륨
    public float maxDistance = 10.0f; // 최대 거리

    void Update()
    {
        AdjustVolumeBasedOnDistance();
    }

    void AdjustVolumeBasedOnDistance()
    {
        float closestDistance = float.MaxValue;

        foreach (Transform obj in objects)
        {
            float distance = Vector3.Distance(player.position, obj.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        // 거리가 maxDistance를 초과하면 minVolume, 거리가 가까울수록 maxVolume에 가까워짐
        float volume = Mathf.Lerp(maxVolume, minVolume, closestDistance / maxDistance);
        bgmSource.volume = Mathf.Clamp(volume, minVolume, maxVolume);
    }
}