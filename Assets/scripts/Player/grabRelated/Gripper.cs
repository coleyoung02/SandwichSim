using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
    [SerializeField] private GameObject open;
    [SerializeField] private bool isRight;
    [SerializeField] private GameObject closed;
    [SerializeField] private List<GameObject> nearHand;
    [SerializeField] private List<GameObject> inHand;
    private bool isOpen;
    
    private List<float> distanceList = new List<float>();

    private void Start()
    {
        isOpen = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            
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
        distanceList.Clear();
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
            return false;
        }
        GameObject closestObject = nearHand[minIndex];
        grab(closestObject);
        anything = true;
        return anything;
    }



    public bool tryRelease()
    {
        bool anything = false;
        foreach (GameObject g in inHand)
        {
            release(g);
            anything = true;
        }
        inHand = new List<GameObject>();
        return anything;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Frobbable>() != null)
        {
            nearHand.Add(other.gameObject);
        }
        else if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.GetComponent<Frobbable>() != null)
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

    private void grab(GameObject g)
    {
        g.GetComponent<Frobbable>().Grab(this);
        g.transform.parent = gameObject.transform;
        g.GetComponent<Rigidbody>().useGravity = false;
        inHand.Add(g);
    }

    private void release(GameObject g)
    {
        g.GetComponent<Frobbable>().Release();
        g.transform.parent = null;
        g.GetComponent<Rigidbody>().useGravity = true;
    }
}
