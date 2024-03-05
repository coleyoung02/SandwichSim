using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private BeeController bees;
    private BearController bear;
    private float startX;
    [SerializeField] private float maxX;
    [SerializeField] private bool justAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (justAudio)
        {
            GameInstanceManager.Instance.TurnOnMusic();
            return;
        }
        GameInstanceManager.Instance.TurnOnMusic();
        bees = Resources.FindObjectsOfTypeAll<BeeController>()[0];
        bear = Resources.FindObjectsOfTypeAll<BearController>()[0];
        startX = gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (justAudio)
        {
            return;
        }
        Transform t = gameObject.transform;
        float off = (bees.transform.position.x + bear.transform.position.x + 8) / 2 - t.position.x;
        if (Mathf.Abs(off) > 1.25f && (t.position.x >= startX || off > 0) && (t.position.x <= maxX || off < 0))
        {
            Debug.LogWarning(off);
            float diff = Mathf.Clamp(Mathf.Abs(off), 1.5f, 4f);
            gameObject.transform.position = new Vector3(Mathf.Sign(off) * Time.deltaTime * 1.5f * diff + t.position.x, t.position.y, t.position.z);
        }
    }
}
