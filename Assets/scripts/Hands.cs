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
    private Mode mode;
    [SerializeField] float moveSpeed = .1f;
    [SerializeField] float rotateSpeed = .1f;
    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.xy;
        usingLeft = false;
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 handPos;
        if (usingLeft)
        {
            handPos = left.transform.position;
        }  
        else
        {
            handPos = right.transform.position;
        }
        if (mode == Mode.xy)
        {
            handPos.x += Input.GetAxis("Mouse X") * moveSpeed;
            handPos.y += Input.GetAxis("Mouse Y") * moveSpeed;
            if (usingLeft)
                left.transform.position = handPos;
            else
                right.transform.position = handPos;
        }
        else if (mode == Mode.xz)
        {
            handPos.x += Input.GetAxis("Mouse X") * moveSpeed;
            handPos.z += Input.GetAxis("Mouse Y") * moveSpeed;
            if (usingLeft)
                left.transform.position = handPos;
            else
                right.transform.position = handPos;
        }
        else if (mode == Mode.rot1)
        {
            if (usingLeft)
                left.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), Space.World);
            else
                right.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0), Space.World);
        }
        else if (mode == Mode.rot2)
        {
            if (usingLeft)
                left.transform.Rotate(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")), Space.World);
            else
                right.transform.Rotate(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")), Space.World);
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
