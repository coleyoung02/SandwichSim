using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        ChasePlayerControls c = collision.gameObject.GetComponent<ChasePlayerControls>();
        if (c != null)
        {
            c.bump();
        }
    }
}
