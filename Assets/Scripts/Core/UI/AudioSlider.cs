using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mainAudioMixer;
    [SerializeField] string audioGroupParameterName, labelPrefix;
    [SerializeField] TextMeshProUGUI percentageLabel;

    // Method to set the volume of the audio mixer group for this slider
    public void SetVolume(float sliderValue)
    {
        // Convert sliderValue (0-1) into decibels
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        mainAudioMixer.SetFloat(audioGroupParameterName, dB);

        // Update matching text
        int percent = Mathf.RoundToInt(sliderValue * 100f);
        percentageLabel.text = labelPrefix + ": " + percent + "%";
    }
}
