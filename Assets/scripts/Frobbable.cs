using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frobbable : MonoBehaviour
{

    [SerializeField] private bool isGroceryItem;
    [SerializeField] private GroceryItem itemId;

    public bool IsGrocery()
    {
        return isGroceryItem;
    }

    public GroceryItem GetItem()
    {
        return itemId;
    }
}
