using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GListPaper : HandInteractable
{
    public override void Grab(Gripper g)
    {
        FindFirstObjectByType<GroceryUI>().Grab();
        g.OnTriggerExit(GetComponent<Collider>());
        g.Toggle(true);
        Destroy(gameObject);
    }

    public override bool GetUsable()
    {
        return true;
    }
}
