using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frobbable : MonoBehaviour
{

    [SerializeField] private LayerMask extraIgnoredLayers;
    private Rigidbody rb;
    private bool held;
    private Gripper gripper;
    private Basket basket;
    private bool isInBasket;
    private bool winOnTouch;
    private SandwichAssembly sand;
    private bool usable;
    protected bool highlightDisabled = false;


    [Header ("grocery items")]
    [SerializeField] private GroceryItem itemId;
    [SerializeField] private bool isGroceryItem;
    [SerializeField] private bool isPreppedIngredient;
    [SerializeField] private float soupChance;
    [SerializeField] private GameObject geomoetry;
    [SerializeField] private GameObject soupGeomoetry;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.AddComponent<Highlight>();
        held = false;
        isInBasket = false;
        usable = true;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public virtual void SetHighlight(bool h)
    {
        if (!highlightDisabled)
        {
            if (h)
            {
                gameObject.GetComponent<Highlight>().StartColors();
            }
            else
            {
                gameObject.GetComponent<Highlight>().ResetColors();
            }
        }
    }

    public virtual void Grab(Gripper g)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        held = true;
        gripper = g;
        if (soupChance > 0f)
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r < soupChance)
            {
                isGroceryItem = false;
                geomoetry.SetActive(false);
                soupGeomoetry.SetActive(true);
                soupChance = 0f;
                g.OnTriggerExit(geomoetry.GetComponent<Collider>());
                FindFirstObjectByType<AudioManager>().PlayPooledClip(ClipPool.SOUP);
            }
            else
            {
                soupChance = -1f;
            }
        }
    }

    public bool GetHeld()
    {
        return held;
    }

    public void SetInBasket(Basket b, bool entering)
    {
        isInBasket = entering;
        basket = b;
    }

    public void OnSlice()
    {
        foreach (Gripper g in FindObjectsByType<Gripper>(FindObjectsSortMode.None))
        {
            if (geomoetry != null)
            {
                g.OnTriggerExit(geomoetry.GetComponent<Collider>());
            }
            else
            {
                g.OnTriggerExit(gameObject.GetComponent<Collider>());
            }
        }
    }

    public void SetUsable(bool u)
    {
        usable = u;
        if (!u)
        {
            foreach (Gripper g in FindObjectsByType<Gripper>(FindObjectsSortMode.None))
            {
                if (geomoetry != null)
                {
                    g.OnTriggerExit(geomoetry.GetComponent<Collider>());
                }
                else
                {
                    g.OnTriggerExit(gameObject.GetComponent<Collider>());
                }
            }
        }
    }

    public bool GetUsable()
    {
        return usable;
    }

    public void Release()
    {

        rb.constraints = RigidbodyConstraints.None;
        held = false;
        if (isInBasket)
        {
            basket.HoldReleased(this);
        }
    }

    public bool IsGrocery()
    {
        return isGroceryItem;
    }

    public bool IsPrepped()
    {
        return isPreppedIngredient;
    }

    public GroceryItem GetItem()
    {
        return itemId;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (winOnTouch && hasLayer(LayerMask.GetMask("PreppedIngredient"), collision.gameObject.layer))
        {
            gripper.Toggle(true);
            sand.OnWin();
        }
        if (held && !collision.gameObject.layer.Equals(LayerMask.GetMask("Hand")))
        {
            if (hasLayer(extraIgnoredLayers, collision.gameObject.layer))
            {
                return;
            }
            gripper.Toggle(true);
        }
    }

    public static bool hasLayer(LayerMask mask, int layer)
    {
        return (mask & (1 << layer)) != 0;
    }

    public void slice(GameObject oldG, GameObject newG)
    {
        oldG.SetActive(false);
        newG.SetActive(true);
    }

    public void WinOnTouch(SandwichAssembly s)
    {
        winOnTouch = true;
        sand = s;
    }
}
