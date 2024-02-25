using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascension : Frobbable
{
    private PlayerController player;
    [SerializeField] private AudioSource ascensionSource;
    [SerializeField] private float upSpeed;
    [SerializeField] private float spinRate;
    private GameObject leftRoof;
    private GameObject rightRoof;
    private GameObject beams;

    private bool activated = false;

    protected override void Start()
    {
        base.Start();
        leftRoof = GameObject.Find("leftRotatePoint");
        rightRoof = GameObject.Find("rightRotatePoint");
        beams = GameObject.Find("topBeams");
    }

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
        beams.SetActive(false);
        StartCoroutine(MoveAway());
    }

    public override re

    private IEnumerator MoveAway()
    {
        for (float i = 0; i < 65; i += Time.deltaTime * 5f)
        {
            leftRoof.transform.rotation = Quaternion.Euler(0, 0, i);
            rightRoof.transform.rotation = Quaternion.Euler(0, 0, -i);
            yield return new WaitForEndOfFrame();
        }
        leftRoof.transform.rotation = Quaternion.Euler(0, 0, 65);
        rightRoof.transform.rotation = Quaternion.Euler(0, 0, -65);
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
