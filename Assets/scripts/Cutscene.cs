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
        FBI_One,
        GrocerySubtitles,
        Opening
    }

    [SerializeField] private List<Camera> cameras;
    [SerializeField] private List<string> dialogue;
    [SerializeField] private List<AudioClip> audios;
    [SerializeField] private GameObject textHolder;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private bool lockControls;
    [SerializeField] private bool skippable;
    [SerializeField] private AudioSource source;
    // for shots with no lines
    [SerializeField] private float shotLength;
    // for shots with lines
    [SerializeField] private float charDelay;
    [SerializeField] private float afterTextLength;
    [SerializeField] private GameObject endTeleportLocation;
    [SerializeField] private GameObject trigger;
    [SerializeField] private Scene scene;
    [SerializeField] private GameObject surferCloneTrigger;
    [SerializeField] private List<GameObject> hintText;

    private float timer;
    private int index;
    private Mode cmode;
    private float expectedLength;
    private float charClock;
    private bool charAddingMode = false;
    private PlayerController pc;

    private void OnEnable()
    {
        foreach (Camera c in cameras)
        {
            c.gameObject.SetActive(false);
        }
        cameras[0].gameObject.SetActive(true);
        index = 0;
        cmode = Mode.Read;
        pc = FindFirstObjectByType<PlayerController>();
        if (lockControls)
        {
            pc.LockControls(true);
        }
        pc.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void Update()
    {
        if (index < cameras.Count)
        {
            if (charAddingMode || timer <= 0 || CheckSkip())
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
                    GameObject g = pc.gameObject;
                    Vector3 p = g.transform.position;
                    p.x += Time.deltaTime * 6f;
                    g.transform.position = p;
                }
                else if (scene == Scene.Opening && index == 2)
                {
                    pc.transform.Rotate(new Vector3(0, Time.deltaTime * -90f / (afterTextLength), 0));
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
        if (trigger != null)
            trigger.SetActive(false);

        if (lockControls)
        {
            pc.LockControls(false);
        }
        if (scene == Scene.FBI_One)
        {
            surferCloneTrigger.SetActive(true);
            GameObject g = FindObjectOfType<PlayerController>().gameObject;
            g.transform.position = endTeleportLocation.transform.position;
            g.transform.rotation = endTeleportLocation.transform.rotation;
        }
        else if (scene == Scene.Opening)
        {
            foreach (GameObject g in hintText)
            {
                g.SetActive(true);
            }
        }

        gameObject.SetActive(false);
    }

    private bool CheckSkip()
    {
        return skippable && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
    }

    private void MainUpdateLoop()
    {
        if (cmode == Mode.Char)
        {
            bool skipped = false;
            if (charAddingMode)
            {
                if (CheckSkip())
                {
                    skipped = true;
                    textBox.text = dialogue[index];
                    charClock = expectedLength;
                }
                else
                {
                    textBox.text = dialogue[index].Substring(0, Mathf.Min(Mathf.RoundToInt(dialogue[index].Length * charClock / expectedLength), dialogue[index].Length));
                    charClock += Time.deltaTime;
                }

                if (textBox.text.Length == dialogue[index].Length)
                {
                    timer = afterTextLength;
                    charAddingMode = false;
                }
            }
            else if (timer >= 0)
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
                textBox.text = "";
                cmode = Mode.Char;
                charClock = 0;
                if (index < audios.Count && audios[index] != null)
                {
                    expectedLength = audios[index].length + .05f;
                    source.Stop();
                    source.clip = audios[index];
                    source.Play();
                }
                else
                {
                    expectedLength = charDelay * dialogue[index].Length;
                }
                charAddingMode = true;
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
