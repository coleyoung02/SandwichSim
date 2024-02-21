using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroceryUI : GroceryListUpdateable
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
    private bool started;
    private bool completed;

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

    private void Update()
    {
        if (!completed)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!started)
                {
                    started = true;
                    StartCoroutine(Twist(!groceryListUI.activeSelf));
                }
            }
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
        groceryListRows[index].text = stringAtIndex(index);
    }

    private string stringAtIndex(int index)
    {
        return groceryItemToString(groceryList.GetItemsNeeded()[index]);
    }

    public override void OnCompletion()
    {
        if (!started)
        {
            started = true;
            StartCoroutine(Twist(false));
        }
        completed = true;
    }

    private IEnumerator Twist(bool appear)
    {
        float totalTime;
        if (appear)
        {
            totalTime = .5f;
            groceryListUI.SetActive(true);
            for (float i = 0; i <= totalTime; i += Time.deltaTime)
            {
                groceryListUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(Mathf.Lerp(45, 0, i / totalTime), Mathf.Lerp(90, 0, i / totalTime), Mathf.Lerp(270, 0, i / totalTime));
                yield return new WaitForEndOfFrame();
            }
            groceryListUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            yield return new WaitForEndOfFrame();
            if (completed)
            {
                StartCoroutine(Twist(false));
            }
        }
        else
        {
            totalTime = 1f;
            for (float i = 0; i <= totalTime; i += Time.deltaTime)
            {
                groceryListUI.GetComponent<RectTransform>().rotation = Quaternion.Euler(Mathf.Lerp(0, 45, i / totalTime), Mathf.Lerp(0, 90, i / totalTime), Mathf.Lerp(0, 270, i / totalTime));
                yield return new WaitForEndOfFrame();
            }
            groceryListUI.SetActive(false);
        }
        started = false;
    }

    public override void OnUpdate(int index, bool has)
    {
        if (has)
        {
            MarkItemComplete(index);
        }
        else
        {
            MarkItemNeeded(index);
        }
    }
}
