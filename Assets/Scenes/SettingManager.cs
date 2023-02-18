using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SetVolume(float volume)
    {
        masterMixer.SetFloat("Volume", volume);
    }
}
