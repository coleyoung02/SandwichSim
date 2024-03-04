using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class MouseLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x + Input.GetAxis("Mouse X") * .1f, -8f, 7.3f);
        p.y = Mathf.Clamp(p.y + Input.GetAxis("Mouse Y") * .1f, -4.1f , 4.5f);
        transform.position = p;
    }
}
