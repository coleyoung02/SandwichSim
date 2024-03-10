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
    private PlayerController pc;
    private bool walkingMode = true;


    private void Start()
    {
        pc = FindAnyObjectByType<PlayerController>();
    }

    private void OnEnable()
    {
        walkingMode = true;
        FinishCutscene();
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
        walkingAgent.SetActive(true);
        if (pc == null)
        {
            pc = FindAnyObjectByType<PlayerController>();
        }
        GameObject g = pc.gameObject; 
        g.transform.position = comingFinishTPPoint.transform.position;
        g.transform.rotation = comingFinishTPPoint.transform.rotation;
        pc.LockControls(true, true);
        g.GetComponent<Rigidbody>().velocity = Vector3.right * 7.5f;
    }

    public void Romance()
    {
        if (pc == null)
        {
            pc = FindAnyObjectByType<PlayerController>();
        }
        GameObject g = pc.gameObject;
        Vector3 cutscenePos = romanceCutscene.transform.position;
        Vector3 shiftAmt = g.transform.position - bearTargetPos.transform.position;
        cutscenePos.x += shiftAmt.x;
        cutscenePos.z += shiftAmt.z;
        romanceCutscene.transform.position = cutscenePos;
        walkingMode = false;
        walkingAgent.SetActive(false);
        startTrigger.SetActive(false);
        g.GetComponent<Rigidbody>().velocity = Vector3.zero;
        romanceCutscene.SetActive(true);
    }

    public void Failure()
    {
        walkingMode = false;
        walkingAgent.SetActive(false);
        if (pc == null)
        {
            pc = FindAnyObjectByType<PlayerController>();
        }
        GameObject g = pc.gameObject;
        g.GetComponent<Rigidbody>().velocity = Vector3.zero;
        failureCutscene.SetActive(true);
    }

}
