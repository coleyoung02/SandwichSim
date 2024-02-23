using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStartTrigger : MonoBehaviour
{
    [SerializeField] private bool isStart;
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<ChasePlayerControls>().SetOn(isStart);
    }
}
