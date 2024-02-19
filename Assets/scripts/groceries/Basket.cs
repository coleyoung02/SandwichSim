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
            f.SetInBasket(this, true);
            if (f.GetHeld())
            {
                ;
            }
            else
            {
                f.gameObject.transform.SetParent(transform);
                StartCoroutine(freezeConstraints(f.gameObject.GetComponent<Rigidbody>()));
            }
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
            f.SetInBasket(this, false);
            if (f.GetHeld())
            {
                ;
            }
            else
            {
                f.gameObject.transform.SetParent(null);
                f.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
        
    }

    public void HoldReleased(Frobbable f)
    {
        f.gameObject.transform.SetParent(transform);
        StartCoroutine(freezeConstraints(f.gameObject.GetComponent<Rigidbody>()));
    }

    private IEnumerator freezeConstraints(Rigidbody rb)
    {
        yield return new WaitForSeconds(.85f);
        rb.constraints = RigidbodyConstraints.FreezeAll;
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
