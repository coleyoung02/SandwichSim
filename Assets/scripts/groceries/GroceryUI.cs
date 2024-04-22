using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroceryUI : GroceryListUpdateable
{
    public static string groceryItemToString(GroceryItem item)
    {
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            switch (item)
            {
                case GroceryItem.Bread: return "Pan";
                case GroceryItem.Tomato: return "Tomate";
                case GroceryItem.Lettuce: return "Lechuga";
                case GroceryItem.Pickle: return "Pepinillo";
                case GroceryItem.Cheese: return "Queso";
                default: return "no hablo español";
            }
        }
        else
        {
            switch (item)
            {
                case GroceryItem.Bread: return "Bread";
                case GroceryItem.Tomato: return "Tomato";
                case GroceryItem.Lettuce: return "Lettuce";
                case GroceryItem.Pickle: return "Pickle";
                case GroceryItem.Cheese: return "Cheese";
                default: return "oh no";
            }
        }
        
    }

    [SerializeField] private Color completeColor;
    [SerializeField] private GameObject groceryListUI;
    [SerializeField] private GroceryList groceryList;
    [SerializeField] private GameObject groceryListRow;
    private List<TextMeshProUGUI> groceryListRows;
    private bool grabbed;
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
        completed = false;
        grabbed = false;
    }

    public void Grab()
    {
        grabbed = true;

        StartCoroutine(Twist(true));
    }

    private void Update()
    {
        if (grabbed && !completed)
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
        groceryListRows[index].text = "<s>" + stringAtIndex(index) + "</s>";
        groceryListRows[index].color = completeColor;
    }

    public void MarkItemNeeded(int index)
    {
        groceryListRows[index].text = stringAtIndex(index);
        groceryListRows[index].color = Color.black;
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
                groceryListUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Mathf.Lerp(45, 0, i / totalTime), Mathf.Lerp(90, 0, i / totalTime), Mathf.Lerp(270, 0, i / totalTime));
                yield return new WaitForEndOfFrame();
            }
            groceryListUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
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
                groceryListUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Mathf.Lerp(0, 45, i / totalTime), Mathf.Lerp(0, 90, i / totalTime), Mathf.Lerp(0, 270, i / totalTime));
                yield return new WaitForEndOfFrame();
            }
            groceryListUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(45, 90, 270);
            yield return new WaitForEndOfFrame();
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
