using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCam : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;
    [SerializeField] private GameObject actualCam;
    private bool isOn = false;

    public void SetOn(bool on)
    {
        isOn = on;
        actualCam.SetActive(on);
        if (on)
        {
            player = FindFirstObjectByType<PlayerController>().gameObject;
            offset = transform.position - player.transform.position;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isOn)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
