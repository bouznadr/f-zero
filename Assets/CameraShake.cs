using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [Tooltip("The noiseComponent component of the virtualCam")]
    private CinemachineBasicMultiChannelPerlin noiseComponent;

    [Header("Dynamic camera settings")]
    [Tooltip("The default frequency gain of the camera.")]
    private float defaultFrequencyGain = 1f;
    [Tooltip("The default amplitude gain of the camera.")]
    private float defaultAmplitudeGain = 1f;
    [Tooltip("How fast will it shake")]
    [SerializeField] private float maxFrequencyGain = 3f;
    [Tooltip("Size of the shaking")]
    [SerializeField] private float maxAmplitudeGain = 1f;
    [Tooltip("The speed of the lerp between gain changes.")]
    [SerializeField] private float gainSpeed = 5f;
    [SerializeField] private float magnitudeScale = 1f;

    private float previousMagnitude = 0f;

    private void Start()
    {
        if (virtualCam == null)
            virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();

        if (noiseComponent == null)
            noiseComponent = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        defaultFrequencyGain = noiseComponent.m_FrequencyGain;
        defaultAmplitudeGain = noiseComponent.m_AmplitudeGain;
    }

    private void FixedUpdate()
    {
        // Calculate the currentMagnitude of the rigidbody
        float currentMagnitude = rb.velocity.magnitude;
        // Calculate the difference between the previous currentMagnitude and the current currentMagnitude
        float magnitudeDifference = Mathf.Abs(currentMagnitude - previousMagnitude);
        // Set the previous currentMagnitude to the current currentMagnitude
        previousMagnitude = currentMagnitude;

        // Calculate the frequency gain based on the magnitude
        float frequencyGain = Mathf.LerpUnclamped(defaultFrequencyGain, maxFrequencyGain, magnitudeDifference * magnitudeScale);
        // Calculate the amplitude gain based on the magnitude
        float amplitudeGain = Mathf.LerpUnclamped(defaultAmplitudeGain, maxAmplitudeGain, magnitudeDifference * magnitudeScale);

        // Set the frequency gain of the noise component
        noiseComponent.m_FrequencyGain = Mathf.Lerp(noiseComponent.m_FrequencyGain, frequencyGain, Time.deltaTime * gainSpeed);
        // Set the amplitude gain of the noise component
        noiseComponent.m_AmplitudeGain = Mathf.Lerp(noiseComponent.m_AmplitudeGain, amplitudeGain, Time.deltaTime * gainSpeed);
    }
}