using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResetSetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (!pc.GetDeceased())
        {
            pc.SetRoadSide(Vector3.Dot(transform.position - pc.gameObject.transform.position, transform.forward) < 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (!pc.GetDeceased())
        {
            pc.SetRoadSide(Vector3.Dot(transform.position - pc.gameObject.transform.position, transform.forward) < 0);
        }
    }
}
