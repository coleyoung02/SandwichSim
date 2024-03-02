using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : HandInteractable
{
    [SerializeField] private GameObject door;
    [SerializeField] private float openDelay;
    private bool isOpen = false;
    private bool swinging = false;
    private float angle = 0f;

    public override void Grab(Gripper g)
    {
        if (!swinging)
        {
            if (isOpen)
            {
                door.transform.localRotation = Quaternion.Euler(0, 0f, 0);
            }
            else
            {
                door.transform.localRotation = Quaternion.Euler(0, 90f, 0);
            }
            isOpen = !isOpen;
            swinging = true;
            StartCoroutine(Swing());
        }
        g.Toggle(true);
    }

    private IEnumerator Swing()
    {
        if (isOpen)
        {
            for (float f = 0f; f < 90f; f += Time.deltaTime / openDelay * 90f)
            {
                yield return new WaitForEndOfFrame();
                angle = f;
                door.transform.localRotation = Quaternion.Euler(0, angle, 0);
            }
            yield return new WaitForEndOfFrame();
            angle = 90f;
            door.transform.localRotation = Quaternion.Euler(0, angle, 0);
            swinging = false;
        }
        else
        {
            for (float f = 90f; f > 0f; f -= Time.deltaTime / openDelay * 90f)
            {
                yield return new WaitForEndOfFrame();
                angle = f;
                door.transform.localRotation = Quaternion.Euler(0, angle, 0);
            }
            yield return new WaitForEndOfFrame();
            angle = 0f;
            door.transform.localRotation = Quaternion.Euler(0, angle, 0);
            swinging = false;
        }
    }

    public override bool GetUsable()
    {
        return true;
    }
}
