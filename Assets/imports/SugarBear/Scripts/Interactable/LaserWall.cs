using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWall : Interactable
{
    [SerializeField] private GameObject lasers;
    [SerializeField] private int DelayInt;
    [SerializeField] private bool DelayB;
    private bool State;

    void Start()
    {
        currentState = baseState;
        lasers.SetActive(currentState);
    }

    public override void trigger(bool state)
    {
        State = state;
        if (isToggle)
        {
            if (state)
            {
                currentState = !currentState;
                lasers.SetActive(currentState);
            }
        }
        else
        {
            StartCoroutine(Activate());
            lasers.SetActive(state ^ baseState);
        }
    }

    IEnumerator Activate()
    {
        if (DelayB && State)
        {
            yield return new WaitForSeconds(DelayInt);
            lasers.SetActive(State ^ baseState);
        }
    }
}
