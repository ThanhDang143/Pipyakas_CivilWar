using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeCamera : MonoBehaviour
{
    public static ShakeCamera ins;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    public static float time;
    public static float intensity;


    void Awake()
    {
        ins = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 5;
    }

    public IEnumerator IEShake(float i, float t)
    {
        time = t;
        intensity = i;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
}
