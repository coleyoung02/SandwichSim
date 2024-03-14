using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
    [SerializeField] private GameObject open;
    [SerializeField] private bool isRight;
    [SerializeField] private GameObject closed;
    private List<GameObject> nearHand;
    private GameObject inHand;
    private bool isOpen;
    private GameObject closest;
    private bool activity;
    

    private void Start()
    {
        isOpen = true;
        closest = null;
        nearHand = new List<GameObject>();
    }

    public void SetActivity(bool a)
    {
        if (a == activity) return;
        activity = a;
        if (a)
        {
            closest = null;
            if (inHand!= null)
            {
                inHand.GetComponent<HandInteractable>().SetHighlight(true);
                closest = inHand;
            }
        }
        else
        {
            if (closest != null)
            {
                closest.GetComponent<HandInteractable>().SetHighlight(false);
            }
        }
    }

    public GameObject GetHeldObject()
    {
        return inHand;
    }

    private void Update()
    {
        if (activity && isOpen)
        {
            GameObject lastFrameClosest = closest;
            closest = getClosest();
            if (lastFrameClosest != closest)
            {
                if (lastFrameClosest != null)
                {
                    lastFrameClosest.GetComponent<HandInteractable>().SetHighlight(false);
                }
                if (closest != null)
                {
                    closest.GetComponent<HandInteractable>().SetHighlight(true);
                }
            }
        }
    }

    public void Toggle(bool forceRelease=false)
    {
        if (forceRelease)
        {
            isOpen = true;
            tryRelease();
            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
        }
        else
        {
            isOpen = !isOpen;

            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
            if (!isOpen)
            {
                tryGrab();
            }
            else
            {
                tryRelease();
            }
        }
    }

    public bool tryGrab()
    {
        if (closest != null)
        {
            grab(closest);
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool HoldingBasket()
    {
        return inHand != null && inHand.tag.Equals("basket");
    }

    private GameObject getClosest()
    {
        bool anything = false;
        float minDistance = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < nearHand.Count; ++i)
        {
            //Error on this line
            float distance;
            try
            {
                distance = (this.gameObject.transform.position - nearHand[i].transform.position).magnitude;
            }
            catch
            {
                nearHand = new List<GameObject>();
                return null;
            }
            if (!anything || distance < minDistance)
            {
                anything = true;
                minIndex = i;
                minDistance = distance;
            }
        }
        if (!anything)
        {
            return null;
        }
        return nearHand[minIndex];
    }

    public bool tryRelease()
    {
        bool anything = false;
        if (inHand != null)
        {
            release(inHand);
            anything = true;
        }
        inHand = null;
        return anything;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HandInteractable>() != null && other.gameObject.GetComponent<HandInteractable>().GetUsable())
        {
            nearHand.Add(other.gameObject);
        }
        else if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.GetComponent<HandInteractable>() != null
             && other.gameObject.transform.parent.gameObject.GetComponent<HandInteractable>().GetUsable())
        {
            nearHand.Add(other.gameObject.transform.parent.gameObject);

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (nearHand.Contains(other.gameObject))
        {
            nearHand.Remove(other.gameObject);
        }
        else if (other.gameObject.transform.parent != null && nearHand.Contains(other.gameObject.transform.parent.gameObject))
        {
            nearHand.Remove(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnRemove(GameObject g)
    {
        g.GetComponent<HandInteractable>().SetHighlight(false);
    }

    private void grab(GameObject g)
    {
        HandInteractable h = g.GetComponent<HandInteractable>();
        h.Grab(this);
        if (h is Frobbable)
        {
            g.transform.parent = gameObject.transform;
            g.GetComponent<Rigidbody>().useGravity = false;
            inHand = g;
        }
    }

    private void release(GameObject g)
    {
        g.transform.parent = null;
        g.GetComponent<Rigidbody>().useGravity = true;
        g.GetComponent<Frobbable>().Release();
    }
}
