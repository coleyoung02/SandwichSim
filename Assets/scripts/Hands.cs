using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    private enum Mode
    {
        xy,
        xz,
        rot1,
        rot2
    }
    [SerializeField] private Gripper left;
    [SerializeField] private Gripper right;
    private bool usingLeft;
    private bool inUse;
    private Mode mode;
    [SerializeField] float moveSpeed = .1f;
    [SerializeField] float rotateSpeed = .1f;
    // Start is called before the first frame update
    void Start()
    {
        inUse = false;
        mode = Mode.xy;
        usingLeft = false;
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        if (inUse)
        {
            mainUpdateLoop();
        }
    }

    public void Toggle()
    {
        inUse = !inUse;
    }

    private void mainUpdateLoop()
    {
        Vector3 handPos;
        if (usingLeft)
        {
            handPos = left.transform.localPosition;
        }
        else
        {
            handPos = right.transform.localPosition;
        }
        if (mode == Mode.xy)
        {
            handPos.x += Input.GetAxis("Mouse X") * moveSpeed;
            handPos.y += Input.GetAxis("Mouse Y") * moveSpeed;
            if (usingLeft)
                left.transform.localPosition = handPos;
            else
                right.transform.localPosition = handPos;
        }
        else if (mode == Mode.xz)
        {
            handPos.x += Input.GetAxis("Mouse X") * moveSpeed;
            handPos.z += Input.GetAxis("Mouse Y") * moveSpeed;
            if (usingLeft)
                left.transform.localPosition = handPos;
            else
                right.transform.localPosition = handPos;
        }
        else if (mode == Mode.rot1)
        {
            if (usingLeft)
            {
                left.transform.RotateAround(left.transform.position, left.transform.forward, Input.GetAxis("Mouse Y"));
                left.transform.RotateAround(left.transform.position, left.transform.right, -Input.GetAxis("Mouse X"));
            }
            else
            {
                right.transform.RotateAround(right.transform.position, right.transform.forward, Input.GetAxis("Mouse Y"));
                right.transform.RotateAround(right.transform.position, right.transform.right, Input.GetAxis("Mouse X"));
            }
        }
        else if (mode == Mode.rot2)
        {
            if (usingLeft)
            {
                left.transform.RotateAround(left.transform.position, left.transform.forward, Input.GetAxis("Mouse Y"));
                left.transform.RotateAround(left.transform.position, left.transform.up, Input.GetAxis("Mouse X"));
            }
            else
            {
                right.transform.RotateAround(right.transform.position, right.transform.forward, Input.GetAxis("Mouse Y"));
                right.transform.RotateAround(right.transform.position, right.transform.up, Input.GetAxis("Mouse X"));
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (usingLeft)
            {
                left.Toggle();
            }
            else
            {
                right.Toggle();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            mode = Mode.xy;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            mode = Mode.xz;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            mode = Mode.rot1;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            mode = Mode.rot2;
        }
        if (Input.GetMouseButton(0))
        {
            usingLeft = true;
        }
        if (Input.GetMouseButton(1))
        {
            usingLeft = false;
        }
    }
}
