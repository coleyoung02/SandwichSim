using System.Collections;
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
    SANDWICH
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<AudioSource> VOsources;
    [SerializeField] private List<AudioSource> SFXsources;
    [SerializeField] private List<AudioSource> MUSICsources;


    [Header ("Clips")]
    [SerializeField] private List<AudioClip> soupClips;
    [SerializeField] private List<AudioClip> sandwichClips;


    private Dictionary<Channel, List<AudioSource>> sources;
    private Dictionary<Channel, int> indicies;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
            setupSources();
        }
    }


    public void PlayClip(Channel channel, AudioClip clip)
    {
        sources[channel][indicies[channel]].clip = clip;
        sources[channel][indicies[channel]].Play();
        indicies[channel] = (indicies[channel] + 1) % sources[channel].Count;
    }

    public void PlayPooledClip(ClipPool pool)
    {
        switch (pool)
        {
            case ClipPool.SOUP: PlayClip(Channel.VO, soupClips[randomIndex(soupClips.Count)]); return;
            case ClipPool.SANDWICH: PlayClip(Channel.VO, sandwichClips[randomIndex(sandwichClips.Count)]); return;
            default: break;
        }
    }

    private int randomIndex(int size)
    {
        return UnityEngine.Random.Range(0, size);
    }

    private void setupSources()
    {
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
