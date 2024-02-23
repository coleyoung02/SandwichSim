using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChasePlayerControls : MonoBehaviour
{

    private enum move
    {
        JUMP,
        SLIDE,
        LEFT,
        RIGHT
    }
    [SerializeField] private float manueverInTime;
    [SerializeField] private float manueverOutTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowedSpeed;
    [SerializeField] private float slowDuration;
    [SerializeField] private float xShift;
    [SerializeField] private float bumpAmount;
    [SerializeField] private float startingOffset;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject feds;
    [SerializeField] private float fedSpeed;
    [SerializeField] private AudioClip clang;

    private bool manuevering;
    private bool manueveringOut;
    private bool jumping;
    private bool slowed;

    private float manueverInClock;
    private float manueverOutClock;
    private move curMov;
    private float baseX;
    private float curSpeed;
    private float fedZOffest;
    private float prevFedZ;

    private Vector3 shift;

    // Update is called once per frame
    void Update()
    {
        if (!manuevering && !jumping)
        {

            rb.velocity = new Vector3(0f, rb.velocity.y, curSpeed);
            if (Input.GetKeyDown(KeyCode.W))
            {
                manuevering = true;
                jumping = true;
                rb.AddForce(Vector3.up * 50f, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                SetMove(move.SLIDE);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                SetMove(move.LEFT);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                SetMove(move.RIGHT);
            }
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, curSpeed);
            if (manueverInClock > 0f)
            {
                manueverInClock = Mathf.Max(manueverInClock - Time.deltaTime, 0f);
                if (curMov == move.SLIDE)
                {
                    rb.gameObject.transform.rotation = Quaternion.Euler(Mathf.Lerp(72.5f, 0f, manueverInClock / manueverInTime), 0f, 0f);
                }
                else if (curMov == move.LEFT)
                {
                    Vector3 p = rb.gameObject.transform.position;
                    p.x = Mathf.Lerp(baseX - xShift, baseX, manueverInClock / manueverInTime);
                    rb.gameObject.transform.position = p;
                }
                else if (curMov == move.RIGHT)
                {
                    Vector3 p = rb.gameObject.transform.position;
                    p.x = Mathf.Lerp(baseX + xShift, baseX, manueverInClock / manueverInTime);
                    rb.gameObject.transform.position = p;
                }
            }
            else if (manueveringOut || !checkSameInput())
            {
                manueveringOut = true;
                manueverOutClock = Mathf.Max(manueverOutClock - Time.deltaTime, 0f);
                if (curMov == move.SLIDE)
                {
                    rb.gameObject.transform.rotation = Quaternion.Euler(Mathf.Lerp(0f, 72.5f, manueverOutClock / manueverOutTime), 0f, 0f);
                }
                else if (curMov == move.LEFT)
                {
                    Vector3 p = rb.gameObject.transform.position;
                    p.x = Mathf.Lerp(baseX, baseX - xShift, manueverOutClock / manueverOutTime);
                    rb.gameObject.transform.position = p;
                }
                else if (curMov == move.RIGHT)
                {
                    Vector3 p = rb.gameObject.transform.position;
                    p.x = Mathf.Lerp(baseX, baseX + xShift, manueverOutClock / manueverOutTime);
                    rb.gameObject.transform.position = p;
                }
                if (manueverOutClock == 0f)
                {
                    manueveringOut = false;
                    manuevering = false;
                }
            }
        }
        UpdateFeds();
        if (fedZOffest <- .001f)
        {
        }
        rb.AddForce(Vector3.down * 30);
    }

    private bool checkSameInput()
    {
        if (curMov == move.LEFT && Input.GetKey(KeyCode.A))
        {
            Debug.Log("LEFT TRUE");
            return true;
        }
        if (curMov == move.RIGHT && Input.GetKey(KeyCode.D))
        {
            Debug.Log("RIGHT TRUE");
            return true;
        }
        if (curMov == move.SLIDE && Input.GetKey(KeyCode.S))
        {
            Debug.Log("DOWN TRUE");
            return true;
        }
        return false;
    }

    private void UpdateFeds()
    {
        Vector3 p = feds.transform.position;
        p.z = transform.position.z - fedZOffest;
        feds.transform.position = p;
    }



    private void Start()
    {
        SetOn(true);
    }

    public void SetOn(bool on)
    {
        if (on)
        {
            manuevering = false;
            jumping = false;
            rb.velocity = new Vector3(0f, 0f, moveSpeed);
            rb.rotation = Quaternion.identity;
            baseX = rb.position.x;
            shift = Vector3.zero;
            curSpeed = moveSpeed;
            fedZOffest = startingOffset;
            prevFedZ = fedZOffest;
        }
        else
        {
            ;
        }
        playerController.SetChaseMode(on);
    }

    private void SetMove(move m)
    {
        curMov = m;
        manuevering = true;
        manueverInClock = manueverInTime;
        manueverOutClock = manueverOutTime;
    }

    public void bump()
    {
        if (!slowed)
        {
            slowed = true;
            curSpeed = slowedSpeed;
            FindObjectOfType<AudioManager>().PlayClip(Channel.SFX, clang);
            StartCoroutine(slow());
        }
    }

    private IEnumerator slow()
    {
        for (float i = 0; i < slowDuration; i += Time.deltaTime)
        {
            fedZOffest -= Time.deltaTime / slowDuration * bumpAmount;
            yield return new WaitForEndOfFrame();
        }
        fedZOffest = prevFedZ - bumpAmount;
        prevFedZ = fedZOffest;
        curSpeed = moveSpeed;
        slowed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Frobbable.hasLayer(LayerMask.GetMask("Floor"), collision.gameObject.layer) && jumping)
        {
            jumping = false;
            manuevering = false;
        }
    }
}
