using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Sliceable s = other.gameObject.GetComponent<Sliceable>();
        if (s != null)
        {
            s.Cut();
        }
    }
}
