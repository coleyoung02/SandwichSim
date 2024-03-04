using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private bool bear;
    [SerializeField] private LevelEnd end;

    private string playerName;
    private bool isIn;

    private void Start()
    {
        if (bear)
        {
            playerName = "Bear";
        }
        else
        {
            playerName = "Bees";
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerName))
        {
            isIn = true;
            end.tryExit();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerName))
        {
            isIn = false;
        }
    }

    public bool Done()
    {
        return isIn;
    }
}
