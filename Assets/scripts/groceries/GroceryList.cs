using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroceryItem
{
    Bread,
    Tomato,
    Lettuce,
    Pickle
}

public class GroceryList : MonoBehaviour
{
    //do not change the Needed list in the script or it might de-sync with the UI
    [SerializeField] private List<GroceryItem> itemsNeeded;
    [SerializeField] private List<GroceryItem> itemsHad;
    [SerializeField] private GroceryUI ui;

    // Note: * Add function and Remove function require us to  have every like item grouped next to each other in the list *



    // When adding, we need to check if that was an item that we needed
    // one more of, if it is, update the UI at the index of the needed list 
    // that corresponds to the item we just added
    // Ex. if   itemsNeed is {Bread, Bread, Tomato, Pickle, Pickle}
    //          itemsHas is {Bread, Pickle}
    //          and we add a Tomato, we need to call updateUI(2, true)
    // Further, if we have no pickle, and we add one, we should call updateUI(3, true)
    // If we already had a pickle we should call updateUI(4, true)
    // Items may not be sorted in order, so it could be {Bread, Pickle, Tomato, Pickle, Bread}
    // in which case we would need to call updateUI(1, true) to add a first pickle,
    // and updateUI(3, true) to add a second


    // adding this function to count occurrences of an item in a list
    private int CountOccurrences(List<GroceryItem> list, GroceryItem item)
    {
        int count = 0;
        foreach (GroceryItem i in list)
        {
            if (i == item)
            {
                count++;
            }
        }
        return count;
    }

    public void AddItem(GroceryItem item)
    {
        itemsHad.Add(item);
        //do the check here and call the function as needed
        int index = itemsNeeded.IndexOf(item);
        if (index != -1)
        {
            int count = CountOccurrences(itemsHad, item);
            if (CountOccurrences(itemsNeeded, item) >= CountOccurrences(itemsHad, item))
                {
                    updateUI(index + count - 1, true);
                }
        }
    }

    // When removing, we need to check if that was an item that we needed
    // and that we did not have extra, if it is, update the UI at the index of the needed list 
    // that corresponds to the item we just added
    // Ex. if   itemsNeed is {Bread, Bread, Tomato, Pickle, Pickle}
    //          itemsHas is {Bread, Pickle}
    //          and we remove a Pickle, we need to call updateUI(3, false)
    //
    // additional copies of ingredients should be handled similarly to how they are in AddItem
    // Try to always call updateUI with the index of n-th occurence of the corresponding ingredient
    // where n is the number we had before removing
    public void RemoveItem(GroceryItem item)
    {
        itemsHad.Remove(item);
        //do the check here and call the function as needed
        int index = itemsNeeded.IndexOf(item);
        if (index != -1)
        {
            int count = CountOccurrences(itemsHad, item);
            if (CountOccurrences(itemsNeeded, item) > CountOccurrences(itemsHad, item))
                {
                    updateUI(index + count, false);
                }
        }
    }

    public List<GroceryItem> GetItemsHad() 
    { 
        return itemsHad; 
    }

    public List<GroceryItem> GetItemsNeeded()
    {
        return itemsNeeded;
    }

    private void updateUI(int index, bool has)
    {
        if (has)
        {
            ui.MarkItemComplete(index);
        }
        else
        {
            ui.MarkItemNeeded(index);
        }
        
    }
}
