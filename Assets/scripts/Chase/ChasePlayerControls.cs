using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
    [SerializeField] private float jumpForce;
    [SerializeField] private float downForce;

    [SerializeField] private GameObject startSpawn;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject feds;
    [SerializeField] private AudioClip clang;
    [SerializeField] private GameObject loseScreen;

    private bool manuevering;
    private bool manueveringOut;
    private bool jumping;
    private bool slowed;
    private bool isOn;

    private float manueverInClock;
    private float manueverOutClock;
    private move curMov;
    private float baseX;
    private float curSpeed;
    private float fedZOffest;
    private float prevFedZ;

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            if (!manuevering && !jumping)
            {

                rb.velocity = new Vector3(0f, rb.velocity.y, curSpeed);
                BaseInputChecks();
            }
            else
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, curSpeed);
                if (manueverInClock > 0f)
                {
                    ManueverIn();
                }
                else if (manueveringOut || !checkSameInput())
                {
                    ManueverOut();
                }
            }
            UpdateFeds();
            if (fedZOffest < .001f)
            {
                fedZOffest = 1f;
                loseScreen.SetActive(true);
                isOn = false;
                rb.velocity = Vector3.zero;
                StartCoroutine(Rerun());
            }
        }   
    }

    void FixedUpdate()
    {
        if (isOn)
        {
            rb.AddForce(Vector3.down * downForce, ForceMode.Force);
        }
    }

    private IEnumerator Rerun()
    {
        yield return new WaitForSeconds(5f);
        gameObject.transform.position = startSpawn.transform.position;
        gameObject.transform.rotation = startSpawn.transform.rotation;
    }

    private bool checkSameInput()
    {
        if (curMov == move.LEFT && Input.GetKey(KeyCode.A))
        {
            return true;
        }
        if (curMov == move.RIGHT && Input.GetKey(KeyCode.D))
        {
            return true;
        }
        if (curMov == move.SLIDE && Input.GetKey(KeyCode.S))
        {
            return true;
        }
        return false;
    }

    private void BaseInputChecks()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            manuevering = true;
            jumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    private void ManueverIn()
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

    private void ManueverOut()
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

    private void UpdateFeds()
    {
        Vector3 p = feds.transform.position;
        p.z = transform.position.z - fedZOffest;
        feds.transform.position = p;
    }

    public void SetOn(bool on)
    {
        if (on)
        {
            isOn = false;
            manuevering = false;
            jumping = false;
            rb.rotation = Quaternion.identity;
            baseX = rb.position.x;
            curSpeed = moveSpeed;
            fedZOffest = startingOffset;
            prevFedZ = fedZOffest;
            UpdateFeds();
            loseScreen.SetActive(false);
            StartCoroutine(WaitToStart());
        }
        else
        {
            feds.SetActive(false);
            isOn = false;
        }
        playerController.LockControls(on);
    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(1.5f);
        isOn = true;
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
        if (!slowed && isOn)
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
            if ((i > slowDuration *.1 && i < slowDuration * .25) || (i > slowDuration * .35 && i < slowDuration * .5) || (i > slowDuration * .6 && i < slowDuration * .72) || (i > slowDuration * .82 && i < slowDuration * .95))
            {
                playerModel.SetActive(false);
            }
            else
            {
                playerModel.SetActive(true);
            }
            fedZOffest -= Time.deltaTime / slowDuration * bumpAmount;
            yield return new WaitForEndOfFrame();
        }
        playerModel.SetActive(true);
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
