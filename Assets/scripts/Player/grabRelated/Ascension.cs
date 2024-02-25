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
    private float roofAngle = 0f;

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
        player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 3f;
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

        StopAllCoroutines();
        StartCoroutine(MoveAway());
    }

    private IEnumerator MoveAway()
    {
        for (float i = roofAngle; i < 95; i += Time.deltaTime * 5f)
        {
            roofAngle = i;
            leftRoof.transform.rotation = Quaternion.Euler(0, 0, i);
            rightRoof.transform.rotation = Quaternion.Euler(0, 0, -i);
            yield return new WaitForEndOfFrame();
        }
        leftRoof.transform.rotation = Quaternion.Euler(0, 0, 95);
        rightRoof.transform.rotation = Quaternion.Euler(0, 0, -95);
    }
    private IEnumerator MoveTowards()
    {
        for (float i = roofAngle; i > 0; i -= Time.deltaTime * 8f)
        {
            roofAngle = i;
            leftRoof.transform.rotation = Quaternion.Euler(0, 0, i);
            rightRoof.transform.rotation = Quaternion.Euler(0, 0, -i);
            yield return new WaitForEndOfFrame();
        }
        beams.SetActive(true);
        leftRoof.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightRoof.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void Release()
    {
        highlightDisabled = false;
        base.Release();
        player.gameObject.GetComponent<Rigidbody>().useGravity = true;
        activated = false;
        ascensionSource.Pause();
        StopAllCoroutines();
        StartCoroutine(MoveTowards());
    }


    void Update()
    {
        if (activated)
        {
            player.gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        }    
    }
}
