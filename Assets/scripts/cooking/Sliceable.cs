using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField] private GameObject cut;
    [SerializeField] private Frobbable sliceParent;

    public void Cut()
    {
        Instantiate(cut, transform.position, transform.rotation);
        Destroy(sliceParent.gameObject);
    }

    private void Start()
    {
        //StartCoroutine(slice());
    }

    private IEnumerator slice()
    {
        yield return new WaitForSeconds(3f);
        Cut();
    }
}
