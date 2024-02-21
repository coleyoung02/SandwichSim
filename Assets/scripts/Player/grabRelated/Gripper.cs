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
        activity = a;
        if (a)
        {
            closest = null;
            if (inHand!= null)
            {
                inHand.GetComponent<Frobbable>().SetHighlight(true);
                closest = inHand;
            }
        }
        else
        {
            if (closest != null)
            {
                closest.GetComponent<Frobbable>().SetHighlight(false);
            }
        }
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
                    lastFrameClosest.GetComponent<Frobbable>().SetHighlight(false);
                }
                if (closest != null)
                {
                    closest.GetComponent<Frobbable>().SetHighlight(true);
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
            if (isOpen)
            {
                tryGrab();
            }
            else
            {
                tryRelease();
            }
            isOpen = !isOpen;

            open.SetActive(isOpen);
            closed.SetActive(!isOpen);
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

    private GameObject getClosest()
    {
        bool anything = false;
        float minDistance = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < nearHand.Count; ++i)
        {
            float distance = (this.gameObject.transform.position - nearHand[i].gameObject.transform.position).magnitude;
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
        if (other.gameObject.GetComponent<Frobbable>() != null && other.gameObject.GetComponent<Frobbable>().GetUsable())
        {
            nearHand.Add(other.gameObject);
        }
        else if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.GetComponent<Frobbable>() != null
             && other.gameObject.transform.parent.gameObject.GetComponent<Frobbable>().GetUsable())
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
        g.GetComponent<Frobbable>().SetHighlight(false);
    }

    private void grab(GameObject g)
    {
        g.GetComponent<Frobbable>().Grab(this);
        g.transform.parent = gameObject.transform;
        g.GetComponent<Rigidbody>().useGravity = false;
        inHand = g;
    }

    private void release(GameObject g)
    {
        g.transform.parent = null;
        g.GetComponent<Rigidbody>().useGravity = true;
        g.GetComponent<Frobbable>().Release();
    }
}