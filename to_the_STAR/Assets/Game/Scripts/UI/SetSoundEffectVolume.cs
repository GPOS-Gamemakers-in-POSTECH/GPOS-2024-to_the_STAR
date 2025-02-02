using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetSoundEffectVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float value)
    {
        mixer.SetFloat("SoundEffect_Vol", Mathf.Log10(value) * 20);
    }
}
