using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    private enum Mode
    {
        move,
        rotate
    }
    [SerializeField] private Gripper left;
    [SerializeField] private Gripper right;
    private bool usingLeft;
    private bool inUse;
    private Mode mode;
    [SerializeField] float moveSpeed = .2f;
    [SerializeField] float rotateSpeed = .2f;
    private Gripper activeHand;
    private float sensitivity = .5f;
    private PlayerController pc;
    private bool activityOverride = false;

    public void SetHandOverride(bool o)
    {
        activityOverride = o;
    }

    // Start is called before the first frame update
    void Start()
    {
        inUse = false;
        mode = Mode.move;
        usingLeft = false;
        activeHand = right;
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", .5f);
        pc = FindFirstObjectByType<PlayerController>();
    }

    public void SetSensitivity(float s)
    {
        sensitivity = s;
        PlayerPrefs.SetFloat("Sensitivity", s);
    }

    public Gripper GetHandInUse()
    {
        if (usingLeft)
        {
            return left;
        }
        else
        {
            return right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInstanceManager.Instance.GetState() == GameState.Paused)
        {
            return;
        }
        if (inUse && (activityOverride || !pc.GetControlsLocked()))
        {
            mainUpdateLoop();
        }
    }

    public void ForceRelease()
    {
        left.tryRelease();
        right.tryRelease();
    }

    public void Toggle()
    {
        inUse = !inUse;
        right.SetActivity(inUse && !usingLeft);
        left.SetActivity(inUse && usingLeft);
    }

    private void mainUpdateLoop()
    {
        Vector3 handPos;
        handPos = activeHand.transform.localPosition;

        if (mode == Mode.move && !Input.GetKey(KeyCode.LeftShift))
        {
            handPos.x = Mathf.Clamp(handPos.x + Input.GetAxis("Mouse X") * moveSpeed * sensitivity, -handPos.z, handPos.z);
            handPos.y = Mathf.Clamp(handPos.y + Input.GetAxis("Mouse Y") * moveSpeed * sensitivity, -handPos.z/2, handPos.z/2);
            activeHand.transform.localPosition = handPos;
        }
        else if (mode == Mode.move)
        {
            handPos.z = Mathf.Max(.75f, handPos.z + Input.GetAxis("Mouse Y") * moveSpeed * sensitivity);
            handPos.x = Mathf.Clamp(handPos.x + Input.GetAxis("Mouse X") * moveSpeed * sensitivity, -handPos.z, handPos.z);
            handPos.y = Mathf.Clamp(handPos.y, -handPos.z / 2, handPos.z / 2);
            activeHand.transform.localPosition = handPos;
        }
        else if (mode == Mode.rotate)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            if (mouseX != 0 && mouseY != 0)
            {
                if (mouseX / mouseY > 10f)
                {
                    mouseY = 0;
                }
                else if (mouseY / mouseX > 10f)
                {
                    mouseX = 0;
                }
            }
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                activeHand.transform.RotateAround(activeHand.transform.position, activeHand.transform.right, mouseX * rotateSpeed * sensitivity);
                activeHand.transform.RotateAround(activeHand.transform.position, activeHand.transform.forward, mouseY * rotateSpeed * sensitivity);
            }
            else
            {
                activeHand.transform.RotateAround(activeHand.transform.position, activeHand.transform.up, mouseY * rotateSpeed * sensitivity);
                activeHand.transform.RotateAround(activeHand.transform.position, activeHand.transform.forward, mouseX * rotateSpeed * sensitivity);
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            activeHand.Toggle();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            mode = Mode.move;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            mode = Mode.rotate;
        }
        if (Input.GetMouseButtonDown(0))
        {
            activeHand = left;
            usingLeft = true;
            left.SetActivity(true);
            right.SetActivity(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            activeHand = right;
            usingLeft = false;
            left.SetActivity(false);
            right.SetActivity(true);
        }
    }
}
