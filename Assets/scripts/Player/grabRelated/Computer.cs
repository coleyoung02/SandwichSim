using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : HandInteractable
{
    [SerializeField] private GameObject useCam;
    private bool onComp = false;

    public override void Grab(Gripper g)
    {
        onComp = true;
        useCam.SetActive(true);
        GameInstanceManager.Instance.TurnOn();
        g.Toggle(true);
        SetHighlight(false);
    }

    public void TurnOff()
    {
        onComp = false;
        useCam.SetActive(false);
        SetHighlight(true);
    }

    public override bool GetUsable()
    {
        return !onComp;
    }
}
