using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Frobbable
{
    [SerializeField] private GameObject saucePrefab;
    [SerializeField] private Transform tip;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float sauceDelay;
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject playerCam;
    private PlayerController pc;
    private float currentAngle = 270f;
    private float useY;
    private float useZ;
    private Hands hands;
    private float timer = 0f;
    private bool fPressed = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        pc = FindFirstObjectByType<PlayerController>();
        hands = FindFirstObjectByType<Hands>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetHeld() && pc.GetUsingHands() && (hands.GetHandInUse().GetHeldObject() == this.gameObject))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAngle = transform.rotation.eulerAngles.x;
                useY = transform.rotation.eulerAngles.y;
                useZ = transform.rotation.eulerAngles.z;
                fPressed = true;
                hintText.SetActive(false);
            }
            if (Input.GetKey(KeyCode.F))
            {
                currentAngle = Mathf.Max(90f, currentAngle - Time.deltaTime * rotateSpeed);
                Vector3 curRot = transform.rotation.eulerAngles;
                curRot.x = currentAngle;
                transform.rotation = Quaternion.Euler(currentAngle, useY, useZ);
            }
            else
            {
                currentAngle = Mathf.Min(270f, currentAngle + Time.deltaTime * rotateSpeed);
                transform.rotation = Quaternion.Euler(currentAngle, useY, useZ);
            }
            if (currentAngle <= 90.1f && Input.GetKey(KeyCode.F))
            {
                if (timer <= 0f)
                {
                    Instantiate(saucePrefab, tip.position, Quaternion.identity);
                    timer = sauceDelay;
                }
            }
            timer -= Time.deltaTime;
            if (!fPressed)
            {
                hintText.transform.LookAt(hintText.transform.position + playerCam.transform.rotation * Vector3.forward, playerCam.transform.rotation * Vector3.up);
                Vector3 pos = hintText.transform.localPosition;
                pos = .035f * hintText.transform.forward + hintText.transform.up * .02f;
                hintText.transform.localPosition = pos;
            }

        }
    }

    public override void Grab(Gripper g)
    {
        base.Grab(g);
        currentAngle = transform.rotation.eulerAngles.x;
        useY = transform.rotation.eulerAngles.y;
        useZ = transform.rotation.eulerAngles.z;
        if (!fPressed)
        {
            hintText.SetActive(true);
        }
    }
}
