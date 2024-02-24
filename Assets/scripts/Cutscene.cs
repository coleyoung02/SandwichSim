using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cutscene : MonoBehaviour
{

    private enum Mode
    {
        Read,
        Char,
        Wait
    }

    private enum Scene
    {
        FBI_One
    }

    [SerializeField] private List<Camera> cameras;
    [SerializeField] private List<string> dialogue;
    [SerializeField] private GameObject textHolder;
    [SerializeField] private TextMeshProUGUI textBox;
    // for shots with no lines
    [SerializeField] private float shotLength;
    // for shots with lines
    [SerializeField] private float charDelay;
    [SerializeField] private float afterTextLength;
    [SerializeField] private GameObject endTeleportLocation;
    [SerializeField] private GameObject trigger;
    [SerializeField] private Scene scene;
    [SerializeField] private GameObject surferCloneTrigger;

    private float timer;
    private int index;
    private int charIndex;
    private Mode cmode;

    private void OnEnable()
    {
        foreach (Camera c in cameras)
        {
            c.gameObject.SetActive(false);
        }
        index = 0;
        cmode = Mode.Read;
        FindObjectOfType<PlayerController>().LockControls(true);
        FindObjectOfType<PlayerController>().gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Update()
    {
        if (index < cameras.Count)
        {
            if (timer <= 0 || CheckSkip())
            {
                ReadState();
                MainUpdateLoop();
            }
            else 
            {
                timer -= Time.deltaTime;
                if (scene == Scene.FBI_One && index ==  3)
                {
                    foreach (Gripper gr in FindObjectsOfType<Gripper>())
                    {
                        gr.Toggle(true);
                    }
                    GameObject g = FindObjectOfType<PlayerController>().gameObject;
                    Vector3 p = g.transform.position;
                    p.x += Time.deltaTime * 6f;
                    g.transform.position = p;
                }
            }
        }
        else if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            OnEnd();
        }
    }

    private void OnEnd()
    {
        textHolder.SetActive(false);
        trigger.SetActive(false);

        FindObjectOfType<PlayerController>().LockControls(false);
        if (scene == Scene.FBI_One)
        {
            surferCloneTrigger.SetActive(true);
        }
        GameObject g = FindObjectOfType<PlayerController>().gameObject;
        g.transform.position = endTeleportLocation.transform.position;
        g.transform.rotation = endTeleportLocation.transform.rotation;

        gameObject.SetActive(false);
    }

    private bool CheckSkip()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    private void MainUpdateLoop()
    {
        if (cmode == Mode.Char)
        {
            bool skipped = false;
            if (charIndex < dialogue[index].Length)
            {
                if (CheckSkip())
                {
                    skipped = true;
                    textBox.text = dialogue[index];
                    charIndex = dialogue[index].Length;
                }
                else
                {
                    textBox.text += dialogue[index][charIndex];
                    timer = charDelay;
                    charIndex++;
                }

                if (charIndex == dialogue[index].Length)
                {
                    timer = afterTextLength;

                }
            }
            if (timer >= 0)
            {
                if (CheckSkip() && ! skipped)
                {
                    timer = 0;
                    cmode = Mode.Read;
                    index++;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
            else
            {
                cmode = Mode.Read;
                index++;
            }
        }
        else if (cmode == Mode.Wait)
        {
            if (CheckSkip())
            {
                timer = 0;
                cmode = Mode.Read;
                index++;
            }
            else if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                cmode = Mode.Read;
                index++;
            }
        }
    }

    private void ReadState()
    {
        if (cmode == Mode.Read)
        {
            if (dialogue[index] != null && dialogue[index].Length > 0)
            {
                textHolder.SetActive(true);
                charIndex = 0;
                textBox.text = "";
                cmode = Mode.Char;
            }
            else
            {
                timer = shotLength;
                textHolder.SetActive(false);
                cmode = Mode.Wait;
            }
            if (index >= 1)
                cameras[index - 1].gameObject.SetActive(false);
            cameras[index].gameObject.SetActive(true);
        }
    }
}
