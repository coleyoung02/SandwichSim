using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Hands hands;
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody rb;
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
            transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Mouse X"));
            Vector3 camRot = cam.gameObject.transform.rotation.eulerAngles;
            float unclamped = xRotation - Input.GetAxis("Mouse Y");
            float newX = Mathf.Clamp(unclamped, -90f, 90f);
            xRotation = newX;
            if (Mathf.Abs(unclamped - newX) > 1f )
            {
                Debug.Log(unclamped);
            }
            cam.transform.localRotation = Quaternion.Euler(newX, 0, 0);
            //should do ground check
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector3((transform.forward * 6).x, rb.velocity.y, (transform.forward * 6).z);
            }
            else if (rb.velocity.magnitude > .02f)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hands.Toggle();
            usingHands = !usingHands;
        }
    }
}
