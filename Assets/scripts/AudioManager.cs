using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public enum Channel
{
    VO,
    SFX,
    MUSIC
}
public enum ClipPool
{
    SOUP,
    SANDWICH,
    PINBALL,
    BUTTON,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private float pitchShiftAmmount;

    [Header ("Clips")]
    [SerializeField] private List<AudioClip> soupClips;
    [SerializeField] private List<AudioClip> sopaClips;
    [SerializeField] private List<AudioClip> sandwichClips;
    [SerializeField] private List<AudioClip> pinballClips;
    [SerializeField] private List<AudioClip> buttonClips;

    [Header ("Sources")]
    [SerializeField] private List<AudioSource> VOsources;
    [SerializeField] private List<AudioSource> SFXsources;
    [SerializeField] private List<AudioSource> MUSICsources;


    private Dictionary<Channel, List<AudioSource>> sources;
    private Dictionary<Channel, int> indicies;
    private bool spanishMode = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("self destroying");
            Destroy(this);
        }
        else
        {
            Debug.Log("initializing");
            Instance = this;

            DontDestroyOnLoad(gameObject);
            setupSources();
        }
    }

    public void SetSpanish(bool isSpanish)
    {
        spanishMode = isSpanish;
    }

    public void PlayClip(Channel channel, AudioClip clip, float sfxShiftOveride=1f)
    {
        AudioSource s = null;
        try
        {
            s = sources[channel][indicies[channel]];
        }
        catch (Exception e) 
        {
            Debug.LogError("channel "  + channel);
            if (indicies == null)
            {
                Debug.Log("null i");
            }
            foreach (KeyValuePair<Channel, int> kvp in indicies)
            {
                Debug.LogWarning(kvp.Key + " " + kvp.Value);
            }
            throw e;
        }
        if (channel == Channel.SFX)
        {
            s.pitch = UnityEngine.Random.Range(1f - pitchShiftAmmount * sfxShiftOveride, 1 + pitchShiftAmmount * sfxShiftOveride);
        }
        s.clip = clip;
        s.Play();
        indicies[channel] = (indicies[channel] + 1) % sources[channel].Count;
    }

    public void PlayPooledClip(ClipPool pool)
    {
        switch (pool)
        {
            case ClipPool.SOUP: if (!spanishMode) { PlayClip(Channel.VO, soupClips[randomIndex(soupClips.Count)]); } else { PlayClip(Channel.VO, sopaClips[randomIndex(sopaClips.Count)]); } return;
            case ClipPool.SANDWICH: PlayClip(Channel.VO, sandwichClips[randomIndex(sandwichClips.Count)]); return;
            case ClipPool.PINBALL: PlayClip(Channel.SFX, pinballClips[randomIndex(pinballClips.Count)]); return;
            case ClipPool.BUTTON: PlayClip(Channel.SFX, buttonClips[randomIndex(buttonClips.Count)], .3f); return;
            default: break;
        }
    }

    private int randomIndex(int size)
    {
        return UnityEngine.Random.Range(0, size);
    }

    private void setupSources()
    {
        Debug.Log("setting up sources");
        sources = new Dictionary<Channel, List<AudioSource>>();
        indicies = new Dictionary<Channel, int>();
        sources[Channel.VO] = VOsources;
        sources[Channel.SFX] = SFXsources;
        sources[Channel.MUSIC] = MUSICsources;
        indicies[Channel.VO] = 0;
        indicies[Channel.SFX] = 0;
        indicies[Channel.MUSIC] = 0;

    }
}
