using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frobbable : HandInteractable
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


    [Header ("grocery items")]
    [SerializeField] private GroceryItem itemId;
    [SerializeField] private bool isGroceryItem;
    [SerializeField] private bool isPreppedIngredient;
    [SerializeField] private float soupChance;
    [SerializeField] private bool isBasket;
    [SerializeField] private GameObject geomoetry;
    [SerializeField] private GameObject soupGeomoetry;

    protected override void Start()
    {
        base.Start();
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


    public override void Grab(Gripper g)
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
                foreach (Gripper gr in FindObjectsByType<Gripper>(FindObjectsSortMode.None))
                {
                    gr.OnTriggerExit(geomoetry.GetComponent<Collider>());
                }
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

    public override bool GetUsable()
    {
        return usable;
    }

    public virtual void Release()
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
        if (isBasket && collision.gameObject.tag.Equals("car") && held)
        {
            FindFirstObjectByType<PlayerController>().OnCollisionEnter(collision);
        }
        else
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
