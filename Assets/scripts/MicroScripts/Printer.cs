using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    [SerializeField] private GameObject groceryList;
    private bool hasPrinted = false;

    public void Print()
    {
        if (!hasPrinted)
        {
            src.Play();
            groceryList.SetActive(true);
            hasPrinted = true;
        }
    }
}
