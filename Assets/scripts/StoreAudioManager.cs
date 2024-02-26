using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StoreAudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> sources;
    [SerializeField] private float lerpInTime;
    [SerializeField] private bool mustExit;
    [SerializeField] private AudioSource StoreVO;
    [SerializeField] private bool wide;
    [SerializeField] private float maxDist;
    [SerializeField] private float minDist;
    private GameObject player;
    private float t;
    private bool isIn;
    private bool hasExited;
    private bool announcementPlaying;

    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        isIn = false;
        announcementPlaying = false;
        SetVolumes(0);
        if (wide)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }
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
                    t = Mathf.Clamp(t + Time.deltaTime, 0, lerpInTime);
                }
                else
                {
                    t = Mathf.Clamp(t - Time.deltaTime, 0, lerpInTime);
                }

            }
            if (Mathf.Abs(t - oldT) >= .0001f)
            {
                UpdateVolume(t);
            }
        }
        
    }

    private void SetVolumes(float v)
    {
        foreach (AudioSource source in sources)
        {
            source.volume = v;
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
