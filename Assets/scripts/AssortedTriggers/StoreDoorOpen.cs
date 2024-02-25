using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StoreDoorOpen : MonoBehaviour
{
    private bool canUse = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float openX;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    private bool opening;

    public void TurnOff()
    {
        canUse = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canUse)
        {
            StartOpen();
        }
    }

    public void Cancel()
    {
        StopAllCoroutines();
        opening = false;
    }

    public void StartOpen()
    {
        if (!opening)
        {
            opening = true;
            StartCoroutine(Open());
        }
    }

    private IEnumerator Open()
    {
        canUse = false;
        Vector3 pos = leftDoor.transform.localPosition;
        for (float f = Mathf.Abs(leftDoor.transform.localPosition.x); f < openX; f += Time.deltaTime * moveSpeed)
        {
            leftDoor.transform.localPosition = new Vector3(-f, pos.y, pos.z);
            rightDoor.transform.localPosition = new Vector3(f, pos.y, pos.z);
            yield return new WaitForEndOfFrame();
        }
        leftDoor.transform.localPosition = new Vector3(-openX, pos.y, pos.z);
        rightDoor.transform.localPosition = new Vector3(openX, pos.y, pos.z);
        opening = false;
    }
}
