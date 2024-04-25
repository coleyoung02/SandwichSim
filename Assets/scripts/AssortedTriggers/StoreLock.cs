using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreLock : GroceryListUpdateable
{

    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private float closedX;
    [SerializeField] private GameObject cutsceneTrigger;
    [SerializeField] private StoreDoorOpen openTrigger;

    private bool hasLocked = false;
    private bool triggered = false;

    public override void OnCompletion()
    {
        if (!triggered)
        {
            cutsceneTrigger.SetActive(true);
            openTrigger.StartOpen();
            triggered = true;
        }
    }

    public override void OnUpdate(int index, bool has)
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasLocked)
        {
            openTrigger.Cancel();
            Vector3 pos = leftDoor.transform.localPosition;
            leftDoor.transform.localPosition = new Vector3(-closedX, pos.y, pos.z);
            rightDoor.transform.localPosition = new Vector3(closedX, pos.y, pos.z);
            hasLocked = true;
        }
    }


}
