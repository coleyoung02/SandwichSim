using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WinEndUI());
    }

    private IEnumerator WinEndUI()
    {
        yield return new WaitForSeconds(5f);
        FindFirstObjectByType<MenuManager>().ShowUI();
    }
}
