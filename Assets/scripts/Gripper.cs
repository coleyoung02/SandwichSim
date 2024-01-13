using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
    [SerializeField] GameObject open;
    [SerializeField] GameObject closed;
    private bool isOpen;

    private void Start()
    {
        isOpen = true;
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        open.SetActive(isOpen);
        closed.SetActive(!isOpen);
    }
}
