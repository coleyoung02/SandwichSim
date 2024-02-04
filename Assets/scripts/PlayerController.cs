using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Hands hands;
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 6;
    private bool usingHands;
    private float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        xRotation = 0f;
        usingHands = false;
    }

    // Update is called once per frame
    void Update()
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
        //either switch this all to force mode, or leave it setting the velocity and do that for all wasd
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector3((transform.forward * moveSpeed).x, rb.velocity.y, (transform.forward * moveSpeed).z);
        }
        else if (rb.velocity.magnitude > .02f)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector3((-transform.forward * moveSpeed).x, rb.velocity.y, (-transform.forward * moveSpeed).z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector3((transform.right * moveSpeed).x, rb.velocity.y, (transform.right * moveSpeed).z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector3((-transform.right * moveSpeed).x, rb.velocity.y, (-transform.right * moveSpeed).z);
        }

    }

}
