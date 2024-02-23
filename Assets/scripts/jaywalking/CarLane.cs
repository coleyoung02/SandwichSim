using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarLane : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private float gap = 30f;
    [SerializeField] private float spawnDistance = 390f;
    [SerializeField] private bool debug;
    private static float yOffset = 3.75f;
    private GameObject lastCar;
    private GameObject player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
        for (float i = 0; i <= 2* spawnDistance; i += gap)
        {
            lastCar = Instantiate(car, getSpawnLoc(true) - i * transform.right, transform.localRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCar != null)
        {
            if (Vector3.Dot(player.transform.position, transform.right) - Vector3.Dot(lastCar.transform.position, transform.right) + gap < spawnDistance)
            {
                lastCar = Instantiate(car, getSpawnLoc(), transform.localRotation);
                if (debug)
                {
                    Debug.LogWarning("car proj = " + Vector3.Dot(lastCar.transform.position, transform.right));
                }
            }
        }
        else
        {
            lastCar = Instantiate(car, getSpawnLoc(), transform.localRotation);
        }
    }

    private Vector3 getSpawnLoc(bool inv=false)
    {
        int mult = 1;
        if (inv)
        {
            mult = -1;
        }
        return transform.forward * Vector3.Dot(transform.forward, transform.position) + 
            Vector3.up * Vector3.Dot(transform.up, transform.position) +
            Vector3.up * yOffset - 
            spawnDistance * transform.right * mult + 
            transform.right * Vector3.Dot(player.transform.position, transform.right);
    }
}
