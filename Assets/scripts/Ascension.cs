using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascension : Frobbable
{
    private PlayerController player;
    [SerializeField] private AudioSource ascensionSource;
    [SerializeField] private float upSpeed;
    [SerializeField] private float spinRate;

    private bool activated = false;


    public override void Grab(Gripper g)
    {
        player = FindFirstObjectByType<PlayerController>();
        base.Grab(g);
        activated = true;
        player.gameObject.GetComponent<Rigidbody>().useGravity = false;
        ascensionSource.Play();
        SetHighlight(false);
        highlightDisabled = true;
        foreach (StoreAudioManager sam in FindObjectsByType<StoreAudioManager>(FindObjectsSortMode.None))
        {
            sam.OnTriggerExit(null);
        }
        foreach (CarLane cl in FindObjectsByType<CarLane>(FindObjectsSortMode.None))
        {
            Destroy(cl.gameObject);
        }
        foreach (Car c in FindObjectsByType<Car>(FindObjectsSortMode.None))
        {
            Destroy(c.gameObject);
        }
    }


    void Update()
    {
        if (activated)
        {
            player.gameObject.transform.position += Vector3.up * Time.deltaTime;
            player.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        }    
    }
}
