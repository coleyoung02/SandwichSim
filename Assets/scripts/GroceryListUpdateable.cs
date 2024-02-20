using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroceryListUpdateable : MonoBehaviour
{
    public abstract void OnCompletion();

    public abstract void OnUpdate(int index, bool has);
}