using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private BeeController bees;
    private BearController bear;
    private float startX;
    [SerializeField] private float maxX;

    // Start is called before the first frame update
    void Start()
    {
        bees = Resources.FindObjectsOfTypeAll<BeeController>()[0];
        bear = Resources.FindObjectsOfTypeAll<BearController>()[0];
        startX = gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = gameObject.transform;
        float off = (bees.transform.position.x + bear.transform.position.x + 8) / 2 - t.position.x;
        if (Mathf.Abs(off) > 2 && (t.position.x >= startX || off > 0) && (t.position.x <= maxX || off < 0))
        {
            gameObject.transform.position = new Vector3(Mathf.Sign(off)*.05f + t.position.x, t.position.y, t.position.z);
        }
    }
}
