using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static string MUS_VOL = "MUS_VOL";
    private static string AMB_VOL = "AMB_VOL";
    private static string SFX_VOL = "SFX_VOL";
    private static string VO_VOL = "VO_VOL";
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider mus;
    [SerializeField] private Slider amb;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider vo;

    void Start()
    {
        mus.value = MixerSet(MUS_VOL, 1f, true);
        amb.value = MixerSet(AMB_VOL, 1f, true);
        sfx.value = MixerSet(SFX_VOL, 1f, true);
        vo.value = MixerSet(VO_VOL, 1f, true);
    }

    public void SetMusicVolume(float v)
    {
        MixerSet(MUS_VOL, v);
    }

    public void SetAmbientVolume(float v)
    {
        MixerSet(AMB_VOL, v);
    }

    public void SetSFXVolume(float v)
    {
        MixerSet(SFX_VOL, v);
    }

    public void SetVOVolume(float v)
    {
        MixerSet(VO_VOL, v);
    }

    private float MixerSet(string s, float v, bool readPrefs=false)
    {
        if (readPrefs)
        {
            float rv = PlayerPrefs.GetFloat(s, v);
            audioMixer.SetFloat(s, Convert(rv));
            return rv;
        }
        else
        {
            audioMixer.SetFloat(s, Convert(v));
            PlayerPrefs.SetFloat(s, v);
            return v;
        }
    }

    private float Convert(float v)
    {
        if (v == 0)
        {
            return -80f;
        }
        else
        {
            return Mathf.Log10(v) * 20;
        }
    }
}
