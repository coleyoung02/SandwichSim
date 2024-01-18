using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frobbable : MonoBehaviour
{
    private Gripper currentGripper;
    private bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        currentGripper = null;
        grabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Gripper[] hands = FindObjectsByType<Gripper>(FindObjectsSortMode.None);
        float minDistance = 9999f;
        Gripper minGripper = null;
        bool grippable = false;
        foreach (Gripper gripper in hands)
        {
            float distance = (gripper.transform.position - transform.position).magnitude;
            if (distance < 2.5f)
            {
                grippable = true;
            }
            if (grippable && distance < minDistance)
            {
                minGripper = gripper;
                minDistance = distance;
            }
        }
        currentGripper = minGripper;
        Debug.Log(minDistance);
        if (Input.GetMouseButtonDown(2) && grippable)
        {
            ToggleGrab(minGripper);
        }
    }

    public void ToggleGrab(Gripper g)
    {
        if (grabbed)
        {
            release();
        }
        else if (currentGripper != null)
        {
            grab(g);
        }
    }

    private void grab(Gripper g)
    {
        transform.parent = g.transform;
        GetComponent<Rigidbody>().useGravity = false;
        grabbed = true;
    }

    private void release()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        grabbed = false;
    }
}
