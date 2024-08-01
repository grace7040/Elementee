using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _musicMasterSlider;
    [SerializeField] Slider _musicBGMSlider;
    [SerializeField] Slider _musicSFXSlider;

    void Awake()
    {
        _musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        _musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    void SetBGMVolume(float volume)
    {
        _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
