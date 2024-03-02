using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    void OnEnable()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
