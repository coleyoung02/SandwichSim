using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frobbable : MonoBehaviour
{

    [SerializeField] private List<LayerMask> extraIgnoredLayers;
    private Rigidbody rb;
    private bool held;
    private Gripper gripper;
    private Basket basket;
    private bool isInBasket;


    [Header ("grocery items")]
    [SerializeField] private GroceryItem itemId;
    [SerializeField] private bool isGroceryItem;
    [SerializeField] private float soupChance;
    [SerializeField] private GameObject geomoetry;
    [SerializeField] private GameObject soupGeomoetry;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        held = false;
        isInBasket = false;
    }

    public void Grab(Gripper g)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        held = true;
        gripper = g;
        if (soupChance > 0f)
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r < soupChance)
            {
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
            g.OnTriggerExit(geomoetry.GetComponent<Collider>());
        }
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

    public GroceryItem GetItem()
    {
        return itemId;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (held && !collision.gameObject.layer.Equals(LayerMask.GetMask("Hand")))
        {
            foreach (LayerMask lm in extraIgnoredLayers)
            {
                if (hasLayer(lm, collision.gameObject.layer))
                {
                    return;
                }
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
}
