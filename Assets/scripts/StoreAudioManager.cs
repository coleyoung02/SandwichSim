using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class StoreAudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> sources;
    [SerializeField] private float lerpInTime;
    [SerializeField] private bool mustExit;
    [SerializeField] private AudioSource StoreVO;
    [SerializeField] private bool wide;
    [SerializeField] private float maxDist;
    [SerializeField] private float minDist;
    [SerializeField] private bool doCrossfade;
    [SerializeField] private float crossfadeDuration;
    private GameObject player;
    private float t;
    private bool isIn;
    private bool hasExited;
    private bool announcementPlaying;
    private int crossfadeIndex;
    private List<float> volumeMultipliers;

    // Start is called before the first frame update
    void Awake()
    {
        t = 0f;
        isIn = false;
        announcementPlaying = false;
        if (wide)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }
        if (doCrossfade)
        {
            volumeMultipliers = new List<float>();
            for (int i = 0; i < sources.Count; ++i)
            {
                volumeMultipliers.Add(0f);
            }
            if (PlayerPrefs.GetInt("Noir", -1) == 1)
            {

                volumeMultipliers[1] = 1f;
                crossfadeIndex = 1;
            }
            else
            {
                volumeMultipliers[0] = 1f;
                crossfadeIndex = 0;
            }
        }
        SetVolumes(0);
    }

    public void SetCrossfadeActive(int active)
    {
        if (!sources[0].isPlaying)
        {
            for (int i = 0; i < volumeMultipliers.Count; ++i)
            {
                volumeMultipliers[i] = 0f;
            }
            volumeMultipliers[active] = 1f;
        }
        crossfadeIndex = active;
    }

    // Update is called once per frame
    void Update()
    {
        float oldT = t;
        if (!announcementPlaying)
        {
            if (wide)
            {
                t = Mathf.Max(maxDist - Mathf.Max(Mathf.Abs(player.transform.position.z - transform.position.z), minDist), 0);
                t /= (maxDist - minDist);
            }
            else
            {
                if (isIn)
                {
                    t = Mathf.Clamp(t + Time.unscaledDeltaTime, 0, lerpInTime);
                }
                else
                {
                    t = Mathf.Clamp(t - Time.unscaledDeltaTime, 0, lerpInTime);
                }

            }
        }
        if (doCrossfade)
        {
            if (crossfadeDuration == 0)
            {
                for (int i = 0; i < volumeMultipliers.Count; ++i)
                {
                    if (i != crossfadeIndex)
                    {
                        volumeMultipliers[i] = 0f;
                    }
                    else
                    {
                        volumeMultipliers[i] = 1f;
                    }
                }
            }
            else
            {
                for (int i = 0; i < volumeMultipliers.Count; ++i)
                {
                    if (i != crossfadeIndex)
                    {
                        volumeMultipliers[i] = Mathf.Max(volumeMultipliers[i] - Time.unscaledDeltaTime / crossfadeDuration, 0f);
                    }
                    else
                    {
                        volumeMultipliers[i] = Mathf.Min(volumeMultipliers[i] + Time.unscaledDeltaTime / crossfadeDuration, 1f);
                    }
                }
            }
        }
        if (doCrossfade || Mathf.Abs(t - oldT) >= .0001f)
        {
            UpdateVolume(t);
        }

    }

    private void SetVolumes(float v)
    {
        for (int i = 0; i < sources.Count; ++i)
        {
            if (doCrossfade)
            {
                float mult = volumeMultipliers[i];
                sources[i].volume = v * mult;
            }
            else
            {
                sources[i].volume = v;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasExited && StoreVO != null)
        {
            StartCoroutine(AfterDelay());
        }
        if (hasExited || !mustExit)
        {
            isIn = true;
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.Play();
                }
            }
        }
    }

    private IEnumerator AfterDelay()
    {
        yield return new WaitForSeconds(lerpInTime * 3.5f);
        announcementPlaying = true;
        for (float i = lerpInTime; i > lerpInTime / 4; i -= Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            UpdateVolume(i);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        StoreVO.Play();
        yield return new WaitForSeconds(StoreVO.clip.length);
        for (float i = lerpInTime / 4; i < lerpInTime; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            UpdateVolume(i);
        }
        announcementPlaying = false;
        UpdateVolume(lerpInTime);
    }

    private void UpdateVolume(float t)
    {
        SetVolumes(Mathf.Lerp(0, 1, t / lerpInTime));
    }

    public void OnTriggerExit(Collider other)
    {
        isIn = false;
        hasExited = true;
    }
}
