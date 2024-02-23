using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnEnter : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private bool activity = true;
    protected virtual void OnTriggerEnter(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(activity);
        }
    }
}
