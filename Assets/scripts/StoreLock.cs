using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLock : GroceryListUpdateable
{

    [SerializeField] private GameObject door;
    [SerializeField] private GroceryList gList;

    private bool hasLocked = false;

    public override void OnCompletion()
    {
        door.SetActive(false);
    }

    public override void OnUpdate(int index, bool has)
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasLocked)
        {
            door.SetActive(true);
        }
    }


}
