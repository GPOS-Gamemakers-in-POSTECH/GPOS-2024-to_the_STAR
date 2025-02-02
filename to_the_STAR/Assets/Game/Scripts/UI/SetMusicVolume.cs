using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetMusicVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float value)
    {
        mixer.SetFloat("BGM_Vol", Mathf.Log10(value) * 20);
    }
}
