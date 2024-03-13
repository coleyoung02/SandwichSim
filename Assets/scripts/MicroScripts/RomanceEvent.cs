using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanceEvent : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Hands hands;
    [SerializeField] private GameObject comingCutscene;
    [SerializeField] private GameObject comingFinishTPPoint;
    [SerializeField] private GameObject romanceCutscene;
    [SerializeField] private GameObject failureCutscene;
    [SerializeField] private GameObject walkingAgent;
    [SerializeField] private GameObject startTrigger;
    [SerializeField] private GameObject bearTargetPos;
    [SerializeField] private GameObject failTrigger;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject courseFinish;
    private PlayerController pc;
    private bool walkingMode = true;
    private bool hasFailed = false;

    private void Start()
    {
        pc = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (walkingMode)
        {
            if (pc == null)
            {
                pc = FindAnyObjectByType<PlayerController>();
            }
            GameObject g = pc.gameObject;
            g.GetComponent<Rigidbody>().velocity = Vector3.right * 7.5f;
        }

    }

    public void FinishCutscene()
    {
        failTrigger.SetActive(true);
        button.SetActive(true);
        walkingMode = true;
        walkingAgent.SetActive(true);
        if (pc == null)
        {
            pc = FindAnyObjectByType<PlayerController>();
        }
        GameObject g = pc.gameObject; 
        g.transform.position = comingFinishTPPoint.transform.position;
        g.transform.rotation = comingFinishTPPoint.transform.rotation;
        pc.LockControls(true, true);
        if (hasFailed)
        {
            g.GetComponent<Rigidbody>().velocity = Vector3.right * 7.5f;
        }
        else
        {
            g.GetComponent<Rigidbody>().velocity = Vector3.right * 12.5f;
        }
    }

    //can only happen once so no need to check if it has already happened
    public void Romance()
    {
        GameObject g = ResetAndShift(romanceCutscene);
        startTrigger.SetActive(false);
        g.GetComponent<Rigidbody>().velocity = Vector3.zero;
        romanceCutscene.SetActive(true);
        courseFinish.SetActive(false);
    }

    public void Failure()
    {
        GameObject g;
        if (hasFailed)
        {
            g = ResetAndShift(failureCutscene);
        }
        else
        {
            g = ResetState();
        }
        hasFailed = true;
        g.GetComponent<Rigidbody>().velocity = Vector3.zero;
        failureCutscene.SetActive(true);
    }

    private GameObject ResetAndShift(GameObject cutscene)
    {
        GameObject g = ResetState();
        Vector3 cutscenePos = cutscene.transform.position;
        Vector3 shiftAmt = g.transform.position - bearTargetPos.transform.position;
        Debug.LogWarning(shiftAmt);
        cutscenePos.x += shiftAmt.x;
        cutscenePos.z += shiftAmt.z;
        cutscene.transform.position = cutscenePos;
        return g;
    }

    private GameObject ResetState()
    {
        if (pc == null)
        {
            pc = FindAnyObjectByType<PlayerController>();
        }
        GameObject g = pc.gameObject;
        failTrigger.SetActive(false);
        button.SetActive(false);
        walkingMode = false;
        walkingAgent.SetActive(false);
        return g;
    }

}
