using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxShift;
    [SerializeField] private float frequency;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private Hands hands;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject realCam;
    [SerializeField] private PinballUI pinballUI;
    [SerializeField] private GameObject deceasedColliders;
    [SerializeField] private DeathCam deathCam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject nearHomeReset;
    [SerializeField] private GameObject nearStoreReset;
    [SerializeField] private MenuManager pause;
    [SerializeField] private GameObject openingCutscene;
    private float sensitivity = .5f;
    private bool usingHands;
    private float xRotation;
    private bool deceased;
    private bool handsOnly;
    private bool controlsLocked;
    private float shiftClock;
    private GameObject lastCarHit;
    private bool isNearHome;
    private Vector3 deathCamPos;
    private Quaternion deathCamRot;
    private bool tpBasket = false;

    void Awake()
    {
        xRotation = 0f;
        usingHands = false;
        deceased = false;
        controlsLocked = false;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", .5f);
        isNearHome = true;
        deathCamPos = deathCam.transform.localPosition;
        deathCamRot = deathCam.transform.localRotation;
        openingCutscene.SetActive(true);
    }

    private void Start()
    {
        GameInstanceManager.Instance.SetPlayer(this);
    }

    public void SetSensitivity(float s)
    {
        sensitivity = s;
        PlayerPrefs.SetFloat("Sensitivity", s);
    }

    public void SetPause(bool p)
    {
        pause.SetPause(p);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("car"))
        {
            if (lastCarHit == null || collision.gameObject != lastCarHit)
            {
                pinballUI.CarHit();
                AudioManager.Instance.PlayPooledClip(ClipPool.PINBALL);
                lastCarHit = collision.gameObject;
                StopAllCoroutines();
                StartCoroutine(WaitABit());
            }
            SetDeceased(true);
            rb.AddForce((collision.gameObject.GetComponent<Rigidbody>().velocity + Vector3.up * collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude / 3) / 3f, ForceMode.Impulse);
            rb.AddTorque(Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity, Vector3.down) * .5f, ForceMode.Impulse);
            tpBasket = false;
            foreach (Gripper gr in FindObjectsByType<Gripper>(FindObjectsSortMode.None))
            {
                if (gr.HoldingBasket())
                {
                    tpBasket = true;
                }
            }
            hands.ForceRelease();
        }
        else
        {
            if (pinballUI.gameObject.activeSelf)
            {
                pinballUI.ResetMult();
            }
        }
    }

    private void SetDeceased(bool d)
    {
        deathCam.SetOn(d);
        deceased = d;
        deceasedColliders.SetActive(d);
        rb.freezeRotation = !d;
        pinballUI.gameObject.SetActive(d);
        if (d)
        {
            deathCam.transform.parent = null;
        }
        else
        {
            deathCam.transform.parent = transform;
            deathCam.transform.localPosition = deathCamPos;
            deathCam.transform.localRotation = deathCamRot;
            pinballUI.ResetAll();
            foreach (Gripper gr in FindObjectsByType<Gripper>(FindObjectsSortMode.None))
            {
                gr.Toggle(true);
            }
        }
    }

    private IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(.03f);
        lastCarHit = null;
    }

    public void SetHandsOnly(bool only)
    {
        if (only)
        {
            usingHands = true;
        }
        handsOnly = true;
    }

    public void LockControls(bool mode, bool allowHands=false)
    {
        if (mode == true && allowHands)
        {
            hands.SetHandOverride(true);
            if (!usingHands)
            {
                usingHands = true;
                hands.Toggle();
            }
        }
        else
        {
            hands.SetHandOverride(false);
        }
        controlsLocked = mode;
    }

    public bool GetControlsLocked() 
    {
        return controlsLocked;
    }

    public bool GetDeceased()
    {
        return deceased;
    }

    public void SetRoadSide(bool home)
    {
        isNearHome = home;
    }

    public bool GetUsingHands()
    {
        return usingHands;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInstanceManager.Instance.GetState() == GameState.Paused)
        {
            return;
        }
        if (!deceased && !controlsLocked)
        {
            if (!usingHands)
            {
                rotate();
                move();
            }
            else
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Space) && !handsOnly)
            {
                hands.Toggle();
                usingHands = !usingHands;
            }

        }
        else if (deceased)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.freezeRotation = true;
                if (isNearHome)
                {
                    transform.rotation = nearHomeReset.transform.rotation;
                    transform.position = nearHomeReset.transform.position;
                    if (tpBasket)
                    {
                        GameObject b = GameObject.FindGameObjectWithTag("basket");
                        b.transform.position = nearHomeReset.transform.position + Vector3.right * 5 + Vector3.forward * 3.5f;
                        b.transform.rotation = nearHomeReset.transform.rotation;
                        Rigidbody brb = b.GetComponent<Rigidbody>();
                        brb.velocity = Vector3.zero;
                        brb.angularVelocity = Vector3.zero;
                    }
                }
                else
                {
                    transform.rotation = nearStoreReset.transform.rotation;
                    transform.position = nearStoreReset.transform.position;
                    if (tpBasket)
                    {
                        GameObject b = GameObject.FindGameObjectWithTag("basket");
                        b.transform.position = nearStoreReset.transform.position - Vector3.right * 5 - Vector3.forward * 3.5f;
                        b.transform.rotation = nearStoreReset.transform.rotation;
                        Rigidbody brb = b.GetComponent<Rigidbody>();
                        brb.velocity = Vector3.zero;
                        brb.angularVelocity = Vector3.zero;
                    }
                }
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                SetDeceased(false);
                CarLane cl1 = FindFirstObjectByType<CarLane>();
                cl1.NukeCars();
                foreach (CarLane cl in FindObjectsByType<CarLane>(FindObjectsSortMode.None))
                {
                    cl.InitializeCars();
                }
            }
        }
        
    }

    private void rotate()
    {
        transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * sensitivity);
        float unclamped = xRotation - Input.GetAxis("Mouse Y") * sensitivity;
        float newX = Mathf.Clamp(unclamped, -90f, 90f);
        xRotation = newX;
        if (Mathf.Abs(unclamped - newX) > 1f)
        {
            Debug.Log(unclamped);
        }
        cam.transform.localRotation = Quaternion.Euler(newX, 0, 0);
    }

    private void move()
    {
        bool noneSet = true;
        Vector3 moveDir = Vector3.zero;
        //either switch this all to force mode, or leave it setting the velocity and do that for all wasd
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += new Vector3((transform.forward * moveSpeed).x, 0, (transform.forward * moveSpeed).z);
            noneSet = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir += new Vector3((-transform.forward * moveSpeed).x, 0, (-transform.forward * moveSpeed).z);
            noneSet = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += new Vector3((transform.right * moveSpeed).x, 0, (transform.right * moveSpeed).z);
            noneSet = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += new Vector3((-transform.right * moveSpeed).x, 0, (-transform.right * moveSpeed).z);
            noneSet = false;
        }
        if (noneSet)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            moveDir = moveDir.normalized * moveSpeed;
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            shiftClock += Time.deltaTime;
            Vector3 cp = cam.transform.localPosition;
            cp.y += Mathf.Sin(shiftClock * Mathf.PI * 2 * frequency * (rb.velocity.magnitude / moveSpeed)) * maxShift / 4;
            cp.x += Mathf.Cos(shiftClock * Mathf.PI * frequency * (rb.velocity.magnitude / moveSpeed)) * maxShift / 4;
            cam.transform.localPosition = cp;
            cp = realCam.transform.localPosition;
            cp.y += Mathf.Sin(shiftClock * Mathf.PI * 2 * frequency * (rb.velocity.magnitude/moveSpeed)) * maxShift / 2;
            cp.x += Mathf.Cos(shiftClock * Mathf.PI * frequency * (rb.velocity.magnitude / moveSpeed)) * maxShift / 2;
            realCam.transform.localPosition = cp;
        }

    }

}
