using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SandwichAssembly : MonoBehaviour
{
    [SerializeField] private GroceryList g;
    [SerializeField] private GameObject finalSandwich;
    private List<GameObject> inside;

    private void Start()
    {
        inside = new List<GameObject>();    
    }

    private void OnTriggerEnter(Collider collision)
    {
        Frobbable f = collision.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsPrepped())
        {
            g.AddItem(f.GetItem());
            inside.Add(f.gameObject);
            if (g.CheckCompletion())
            {
                CheckSandwichness();
            }
        }
    }

    public void OnWin()
    {
        foreach (GameObject go in inside)
        {
            go.GetComponent<Frobbable>().GetRigidbody().constraints = RigidbodyConstraints.FreezeAll;
            go.GetComponent<Frobbable>().SetUsable(false);
            Destroy(go.GetComponent<Rigidbody>());
            go.transform.parent = finalSandwich.transform;
        }
        finalSandwich.AddComponent<Frobbable>();
        finalSandwich.layer = 0;
        finalSandwich.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        finalSandwich.GetComponent<Eat>().SetWinable(true);
    }


    private void OnTriggerExit(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsPrepped())
        {
            g.RemoveItem(f.GetItem());
            inside.Remove(f.gameObject);
        }
    }

    private bool CheckSandwichness()
    {
        inside = inside.OrderBy(x => x.transform.position.y).ToList();
        if (inside[0].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread &&
            inside[inside.Count - 1].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread)
        {
            inside[inside.Count - 1].GetComponent<Frobbable>().WinOnTouch(this);
        }
        return inside[0].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread &&
            inside[inside.Count - 1].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread;
    }
}
