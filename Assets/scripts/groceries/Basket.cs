using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{

    [SerializeField] private GroceryList list;

    private void OnTriggerEnter(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f == null)
            f = GetParentFrob(other);
        if (f != null && f.IsGrocery())
        {
            list.AddItem(f.GetItem());
            f.gameObject.transform.SetParent(transform);
            Debug.Log("added " + GroceryUI.groceryItemToString(f.GetItem()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f == null)
            f = GetParentFrob(other);
        if (f != null && f.IsGrocery())
        {
            list.RemoveItem(f.GetItem());
            Debug.Log("removed " + GroceryUI.groceryItemToString(f.GetItem()));
        }
        
    }

    private Frobbable GetParentFrob(Collider other)
    {
        if (other.gameObject.transform.parent == null)
        {
            return null;
        }
        return other.gameObject.transform.parent.gameObject.GetComponent<Frobbable>();
    }

}
