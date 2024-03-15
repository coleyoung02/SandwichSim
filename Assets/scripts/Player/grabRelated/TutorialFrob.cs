using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialFrob : Frobbable
{
    public override void Grab(Gripper g)
    {
        base.Grab(g);
        FindFirstObjectByType<Tutorial>().NextText(5);
    }

    public override void Release()
    {
        base.Release();
        FindFirstObjectByType<Tutorial>().NextText(6);
    }

}
