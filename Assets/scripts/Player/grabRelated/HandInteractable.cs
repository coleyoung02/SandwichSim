using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandInteractable : MonoBehaviour
{
    protected bool highlightDisabled = false;
    public abstract void Grab(Gripper g);

    protected virtual void Start()
    {
        gameObject.AddComponent<Highlight>();
    }

    public abstract bool GetUsable();

    public virtual void SetHighlight(bool h)
    {
        if (!highlightDisabled)
        {
            if (h)
            {
                gameObject.GetComponent<Highlight>().StartColors();
            }
            else
            {
                gameObject.GetComponent<Highlight>().ResetColors();
            }
        }
    }
}
