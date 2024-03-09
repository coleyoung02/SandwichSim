using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEvent : HandInteractable
{
    [SerializeField] private RomanceEvent rEv;

    public override void Grab(Gripper g)
    {
        rEv.Romance();
        g.OnTriggerExit(GetComponent<Collider>());
        g.Toggle(true);
        Destroy(gameObject);
    }

    public override bool GetUsable()
    {
        return true;
    }
}
