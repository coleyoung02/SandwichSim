using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLane : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private float gap = 30f;
    [SerializeField] private float spawnDistance = 390f;
    private GameObject lastCar;
    private GameObject player;
    private int sign;

    void Start()
    {
        sign = car.GetComponent<Car>().GetSign();
        player = FindFirstObjectByType<PlayerController>().gameObject;
        if (sign == 1)
        {
            for (float i = -spawnDistance; i <= spawnDistance; i += gap)
            {
                lastCar = Instantiate(car, new Vector3(player.transform.position.x + i, transform.position.y + 6f, transform.position.z), Quaternion.identity);
            }
        }
        else
        {
            for (float i = spawnDistance; i >= -spawnDistance; i -= gap)
            {
                lastCar = Instantiate(car, new Vector3(player.transform.position.x + i, transform.position.y + 6f, transform.position.z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCar != null)
        {
            if (sign == 1)
            {
                if (((player.transform.position.x + spawnDistance) - lastCar.transform.position.x) > gap)
                {
                    lastCar = Instantiate(car, new Vector3(player.transform.position.x + spawnDistance, transform.position.y + 6f, transform.position.z), Quaternion.identity);

                }
            }
            else
            {
                if ((lastCar.transform.position.x - (player.transform.position.x - spawnDistance)) > gap)
                {
                    lastCar = Instantiate(car, new Vector3(player.transform.position.x - spawnDistance, transform.position.y + 6f, transform.position.z), Quaternion.identity);

                }
            }
        }
        else
        {
            lastCar = Instantiate(car, new Vector3(player.transform.position.x + sign * spawnDistance, transform.position.y + 6f, transform.position.z), Quaternion.identity);
        }
    }
}
