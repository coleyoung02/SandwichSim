using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRace : ActivateOnEnter
{
    [SerializeField] private GameObject tpPoint;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        FindFirstObjectByType<ChasePlayerControls>().SetOn(false);
        FindFirstObjectByType<PlayerController>().gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        FindFirstObjectByType<PlayerController>().transform.position = tpPoint.transform.position;
        FindFirstObjectByType<PlayerController>().transform.rotation = tpPoint.transform.rotation;
        gameObject.SetActive(false);
    }
}
