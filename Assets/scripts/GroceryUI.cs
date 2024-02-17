using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroceryUI : MonoBehaviour
{
    public static string groceryItemToString(GroceryItem item)
    {
        switch (item) {
            case GroceryItem.Bread: return "bread";
            case GroceryItem.Tomato: return "tomato";
            case GroceryItem.Lettuce: return "lettuce";
            case GroceryItem.Pickle: return "pickle";
            default: return "oh no";
        }
    }

    public static GroceryItem StringToGroceryItem(string s)
    {
        switch (s)
        {
            case "bread": return GroceryItem.Bread;
            case "tomato": return GroceryItem.Tomato;
            case "lettuce": return GroceryItem.Lettuce;
            case "pickle": return GroceryItem.Pickle;
                //might break C#
            default: return (GroceryItem)(-1);
        }
    }

    [SerializeField] private GameObject groceryListUI;
    [SerializeField] private GroceryList groceryList;
    [SerializeField] private GameObject groceryListRow;
    private List<TextMeshProUGUI> groceryListRows;

    private const string START_STRIKETHROUGH_TAG = "<s>";
    private const string END_STRIKETHROUGH_TAG = "</s>";

    void Start()
    {
        groceryListRows = new List<TextMeshProUGUI>();
        foreach (GroceryItem g in groceryList.GetItemsNeeded()) {
            GameObject r = Instantiate(groceryListRow, groceryListUI.transform);
            groceryListRows.Add(r.GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < groceryListRows.Count; ++i)
        {
            MarkItemNeeded(i);
        }
    }

    public void MarkItemComplete(int index)
    {
        groceryListRows[index].text = START_STRIKETHROUGH_TAG +
            stringAtIndex(index) +
            END_STRIKETHROUGH_TAG;
    }

    public void MarkItemNeeded(int index)
    {
        Debug.Log(index);
        groceryListRows[index].text = stringAtIndex(index);
    }

    private string stringAtIndex(int index)
    {
        return groceryItemToString(groceryList.GetItemsNeeded()[index]);
    }
}
