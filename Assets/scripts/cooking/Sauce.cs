using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sauce : Frobbable
{
    [SerializeField] private GameObject saucePrefab;
    [SerializeField] private Transform tip;
    [SerializeField] private float rotateSpeed;
    private PlayerController pc;
    private float currentAngle;
    private Hands hands;
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
                currentAngle = Mathf.Min(90f, currentAngle + Time.deltaTime * rotateSpeed);
            }
            else
            {
                currentAngle = Mathf.Max(-90f, currentAngle - Time.deltaTime * rotateSpeed);
            }
            Vector3 curRot = transform.rotation.eulerAngles;
            curRot.x = currentAngle;
            transform.rotation = Quaternion.Euler(curRot);
            if (currentAngle >= 89.9f && Input.GetKey(KeyCode.F))
            {
                Instantiate(saucePrefab, tip.position, Quaternion.identity);
            }
        }
    }

    public override void Grab(Gripper g)
    {
        base.Grab(g);
        currentAngle = transform.rotation.eulerAngles.x;
    }


}
