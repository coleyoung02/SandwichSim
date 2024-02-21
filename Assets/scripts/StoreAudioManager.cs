using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreAudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> sources;
    [SerializeField] private float lerpInTime;
    [SerializeField] private bool mustExit;
    private float t;
    private bool isIn;
    private bool hasExited;

    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        isIn = false;

        SetVolumes(0);
    }

    // Update is called once per frame
    void Update()
    {
        float oldT = t;
        if (isIn)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, lerpInTime);
        }
        else
        {
            t = Mathf.Clamp(t - Time.deltaTime, 0, lerpInTime);
        }
        if (Mathf.Abs(t - oldT) >= .0001f)
        {
            SetVolumes(Mathf.Lerp(0, 1, t/lerpInTime));
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

    public void OnTriggerExit(Collider other)
    {
        isIn = false;
        hasExited = true;
    }
}
