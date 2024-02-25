using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SandwichAssembly : GroceryListUpdateable
{
    [SerializeField] private GroceryList g;
    [SerializeField] private GameObject finalSandwich;
    private List<GameObject> inside;
    private bool alreadyWon = false;

    private void Start()
    {
        inside = new List<GameObject>();    
    }

    private void OnTriggerEnter(Collider collision)
    {
        Frobbable f = collision.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsPrepped())
        {
            inside.Add(f.gameObject);
            g.AddItem(f.GetItem());
        }
    }



    public void OnWin()
    {
        
        GameObject g = Instantiate(finalSandwich, this.transform.position - Vector3.up *.212f, Quaternion.identity);
        finalSandwich.layer = 0;
        foreach (GameObject go in inside)
        {
            go.GetComponent<Frobbable>().GetRigidbody().constraints = RigidbodyConstraints.FreezeAll;
            go.GetComponent<Frobbable>().SetUsable(false);
            Destroy(go.GetComponent<Rigidbody>());
            go.transform.parent = g.transform;
        }
        //finalSandwich.GetComponent<Eat>().SetWinable(true);
    }


    private void OnTriggerExit(Collider other)
    {
        Frobbable f = other.gameObject.GetComponent<Frobbable>();
        if (f != null && f.IsPrepped())
        {
            g.RemoveItem(f.GetItem());
            inside.Remove(f.gameObject);
        }
        if (g.CheckCompletion() && !alreadyWon)
        {
            if (CheckSandwichness(true))
            {
                OnWin();
            }
        }
    }

    private bool CheckSandwichness(bool skipAdd=false)
    {
        inside = inside.OrderBy(x => x.transform.position.y).ToList();
        if (inside[0].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread &&
            inside[inside.Count - 1].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread &&
            !skipAdd)
        {
            alreadyWon = true;
            inside[inside.Count - 1].GetComponent<Frobbable>().WinOnTouch(this);
        }
        return inside[0].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread &&
            inside[inside.Count - 1].GetComponent<Frobbable>().GetItem() == GroceryItem.Bread;
    }

    public override void OnCompletion()
    {
        CheckSandwichness();
    }

    public override void OnUpdate(int index, bool has)
    {
        return;
    }
}
