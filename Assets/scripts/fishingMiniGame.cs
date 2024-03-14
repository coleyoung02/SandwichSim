using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fishingMiniGame : MonoBehaviour
{

    [SerializeField] GameObject UIElement;
    [SerializeField] GameObject bar;
    [SerializeField] GameObject winObject;
    [SerializeField] Transform spawnLocation;

    private bool canFish = false;
    private bool isFishing = false;

    private void Start()
    {
        UIElement.SetActive(false);
        bar.SetActive(false);

    }

    private void Update()
    {
        if (canFish && Input.GetKeyDown(KeyCode.F))
        {
            if (!isFishing)
            {
                UIElement.SetActive(false);
                isFishing = true;
                bar.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fishingRod")
        {
            if (!isFishing)
            {
                UIElement.SetActive(true);
            }
            canFish = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFishing && Input.GetKeyDown(KeyCode.F) && other.tag == "fish")
        {
            for (int i =0; i<5; i++)
            {
                Instantiate(winObject, 
                    spawnLocation.transform.position + new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f)), 
                    Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f)));
            }
            StartCoroutine(barOff());
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "fishingRod")
        {
            UIElement.SetActive(false);
            canFish = false;
        }
    }

    IEnumerator barOff()
    {
        
        bar.SetActive(false);
        UIElement.SetActive(true);
        yield return new WaitForSeconds(1f);
        isFishing = false;
    }
}
