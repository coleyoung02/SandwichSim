using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroceryUI : MonoBehaviour
{
    [SerializeField] private GameObject groceryListUI;
    [SerializeField] private GroceryList groceryList;
    [SerializeField] private GameObject groceryListRow;
    private List<TextMeshProUGUI> groceryListRows;

    private const string START_STRIKETHROUGH_TAG = "<s>";
    private const string END_STRIKETHROUGH_TAG = "</s>";

    void Start()
    {
        foreach (GroceryItem g in groceryList.GetItemsNeeded()) {
            GameObject r = Instantiate(groceryListRow, groceryListUI.transform);
            groceryListRows.Add(r.GetComponent<TextMeshProUGUI>());
        }
    }

    public void MarkItemComplete(int index)
    {

    }

    public void MarkItemNeeded(int index)
    {

    }
}
