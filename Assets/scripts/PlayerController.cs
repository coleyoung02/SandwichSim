using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Hands hands;
    [SerializeField] private Camera cam;
    [SerializeField] private Camera deathCam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 6;
    private bool usingHands;
    private float xRotation;
    private bool deceased;
    // Start is called before the first frame update
    void Start()
    {
        xRotation = 0f;
        usingHands = false;
        deceased = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("car"))
        {
            deathCam.gameObject.transform.parent = null;
            deathCam.gameObject.SetActive(true);
            deceased = true;
            rb.freezeRotation = false;
            rb.AddForce((collision.gameObject.GetComponent<Rigidbody>().velocity + Vector3.up * 20) / 5f, ForceMode.Impulse);
            rb.AddTorque(Vector3.Cross(collision.gameObject.GetComponent<Rigidbody>().velocity, Vector3.down).normalized * 4, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!deceased)
        {
            if (!usingHands)
            {
                rotate();
                move();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hands.Toggle();
                usingHands = !usingHands;
            }

        }
    }

    private void rotate()
    {
        transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Mouse X"));
        Vector3 camRot = cam.gameObject.transform.rotation.eulerAngles;
        float unclamped = xRotation - Input.GetAxis("Mouse Y");
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
        }

    }

}
