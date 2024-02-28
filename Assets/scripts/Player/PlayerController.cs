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
    [SerializeField] private GameObject deceasedColliders;
    [SerializeField] private float maxShift;
    [SerializeField] private float frequency;
    [SerializeField] private Camera deathCam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 6;
    private float sensitivity = .5f;
    private bool usingHands;
    private float xRotation;
    private bool deceased;
    private bool handsOnly;
    private bool controlsLocked;
    private float shiftClock;

    // Start is called before the first frame update
    void Awake()
    {
        xRotation = 0f;
        usingHands = false;
        deceased = false;
        controlsLocked = false;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", .5f);
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
            AudioManager.Instance.PlayPooledClip(ClipPool.PINBALL);
            deathCam.gameObject.transform.parent = null;
            deathCam.gameObject.SetActive(true);
            deceased = true;
            deceasedColliders.SetActive(true);
            rb.freezeRotation = false;
            rb.AddForce((collision.gameObject.GetComponent<Rigidbody>().velocity + Vector3.up * 15) / 3f, ForceMode.Impulse);
            rb.AddTorque(Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity, Vector3.down).normalized * 16, ForceMode.Impulse);
            hands.ForceRelease();
        }
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
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
            cp.y += Mathf.Sin(shiftClock * Mathf.PI * 2 * frequency) * maxShift / 2;
            cp.x += Mathf.Cos(shiftClock * Mathf.PI * frequency) * maxShift / 2;
            cam.transform.localPosition = cp;
            cp = realCam.transform.localPosition;
            cp.y += Mathf.Sin(shiftClock * Mathf.PI * 2 * frequency) * maxShift / 8;
            cp.x += Mathf.Cos(shiftClock * Mathf.PI * frequency) * maxShift / 8;
            realCam.transform.localPosition = cp;
        }

    }

}
