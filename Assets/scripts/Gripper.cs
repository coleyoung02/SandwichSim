using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper : MonoBehaviour
{
    [SerializeField] private GameObject open;
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

    public void Toggle()
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
        foreach (GameObject g in nearHand)
        {
            float distance = this.gameObject.transform.position - g.gameObject.transform.position;
            distanceList.Add(distance);
            //grab(g);
            //anything = true;
        }
        float minDistance = Mathf.Min(distanceList.ToArray());
        int minIndex = distanceList.IndexOf(minDistance);
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
        if (other.gameObject.tag == "frobbable")
        {
            nearHand.Add(other.gameObject);
            Debug.Log("can grab " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearHand.Contains(other.gameObject))
        {
            nearHand.Remove(other.gameObject);
            Debug.Log("canT grab " + other.gameObject.name);
        }
    }

    private void grab(GameObject g)
    {
        g.transform.parent = gameObject.transform;
        g.GetComponent<Rigidbody>().useGravity = false;
        inHand.Add(g);
    }

    private void release(GameObject g)
    {
        g.transform.parent = null;
        g.GetComponent<Rigidbody>().useGravity = true;
    }
}
