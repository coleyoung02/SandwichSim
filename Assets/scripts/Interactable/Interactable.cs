using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected bool isToggle = false;
    [SerializeField] protected bool baseState = true;
    protected bool currentState;
    void Start()
    {
        currentState = baseState;
    }

    public abstract void trigger(bool state);
}
