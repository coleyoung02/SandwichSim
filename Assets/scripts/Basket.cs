using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{

    [SerializeField] private GroceryList list;

    private void OnTriggerEnter(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsGrocery())
        {
            list.AddItem(f.GetItem());
            Debug.Log("added " + GroceryUI.groceryItemToString(f.GetItem()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsGrocery())
        {
            list.RemoveItem(f.GetItem());
            Debug.Log("removed " + GroceryUI.groceryItemToString(f.GetItem()));
        }
    }


}
