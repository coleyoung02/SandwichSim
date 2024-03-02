using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Hands hands;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject realCam;
    [SerializeField] private PinballUI pinballUI;
    [SerializeField] private GameObject deceasedColliders;
    [SerializeField] private Camera deathCam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject nearHomeReset;
    [SerializeField] private GameObject nearStoreReset;
    [SerializeField] private float maxShift;
    [SerializeField] private float frequency;
    [SerializeField] private float moveSpeed = 6;
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

    // Start is called before the first frame update
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
    }

    public void SetSensitivity(float s)
    {
        sensitivity = s;
        PlayerPrefs.SetFloat("Sensitivity", s);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("car"))
        {
            if (lastCarHit == null || collision.gameObject != lastCarHit)
            {
                Debug.Log(lastCarHit);
                pinballUI.CarHit();
                AudioManager.Instance.PlayPooledClip(ClipPool.PINBALL);
                lastCarHit = collision.gameObject;
                StopAllCoroutines();
                StartCoroutine(WaitABit());
            }
            SetDeceased(true);
            rb.AddForce((collision.gameObject.GetComponent<Rigidbody>().velocity + Vector3.up * 15) / 3f, ForceMode.Impulse);
            rb.AddTorque(Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity, Vector3.down).normalized * 16, ForceMode.Impulse);
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
        deathCam.gameObject.SetActive(d);
        deceased = d;
        deceasedColliders.SetActive(d);
        rb.freezeRotation = !d;
        pinballUI.gameObject.SetActive(d);
        if (d)
        {
            deathCam.gameObject.transform.parent = null;
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

    public void LockControls(bool mode)
    {
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

    // Update is called once per frame
    void Update()
    {
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
                if (isNearHome)
                {
                    transform.rotation = nearHomeReset.transform.rotation;
                    transform.position = nearHomeReset.transform.position;
                }
                else
                {
                    transform.rotation = nearStoreReset.transform.rotation;
                    transform.position = nearStoreReset.transform.position;
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
