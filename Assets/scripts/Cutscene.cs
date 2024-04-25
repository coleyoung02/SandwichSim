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
        Opening,
        Coming,
        RomanceWin,
        RomanceLoss
    }

    [SerializeField] private List<Camera> cameras;
    [SerializeField] private List<string> dialogue;
    [SerializeField] private List<string> dialogueEsp;
    [SerializeField] private List<AudioClip> audios;
    [SerializeField] private List<AudioClip> audiosEsp;
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
    [SerializeField] private GameObject regAgent;
    [SerializeField] private GameObject romancedAgent;

    private float timer;
    private int index;
    private Mode cmode;
    private float expectedLength;
    private float charClock;
    private bool charAddingMode = false;
    private PlayerController pc;

    private void OnEnable()
    {
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            audios = audiosEsp;
            dialogue = dialogueEsp;
        }

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
        if (scene == Scene.Coming)
        {
            FindFirstObjectByType<ChasePlayerControls>().SetOn(false);
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
                    p.x += Time.deltaTime * 10.5f;
                    g.transform.position = p;
                }
                else if (scene == Scene.Opening && index == 2)
                {
                    pc.transform.Rotate(new Vector3(0, Time.deltaTime * -90f / (afterTextLength), 0));
                }
                else if (scene == Scene.RomanceWin && index == 2)
                {
                    pc.transform.Rotate(new Vector3(0, Time.deltaTime * 90f / (afterTextLength), 0));
                }
                if (scene == Scene.RomanceWin && index == 3)
                {
                    pc.gameObject.GetComponent<Rigidbody>().velocity = -Vector3.forward * 50;
                    pc.transform.rotation = Quaternion.Euler(0, 180, Mathf.Sin(Time.time * 3.14f * 6) * 10f);
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
        }
        foreach (GameObject g in hintText)
        {
            g.SetActive(true);
        }
        if (scene == Scene.Coming)
        {
            FindFirstObjectByType<RomanceEvent>().FinishCutscene();
        }
        if (endTeleportLocation != null)
        {
            GameObject g = FindObjectOfType<PlayerController>().gameObject;
            g.transform.position = endTeleportLocation.transform.position;
            g.transform.rotation = endTeleportLocation.transform.rotation;
        }
        

        gameObject.SetActive(false);
    }

    private bool CheckSkip()
    {
        return skippable && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.timeScale >= .01f;
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
                    if (index == 0 && scene == Scene.RomanceWin)
                    {
                        regAgent.SetActive(false);
                        romancedAgent.SetActive(true);
                    }
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
                if (index < audios.Count && audios[index] != null)
                {
                    timer = audios[index].length + .05f;
                    source.Stop();
                    source.clip = audios[index];
                    source.Play();
                }
                else
                {
                    timer = shotLength;
                }
                textHolder.SetActive(false);
                cmode = Mode.Wait;
            }
            if (index >= 1)
                cameras[index - 1].gameObject.SetActive(false);
            cameras[index].gameObject.SetActive(true);
        }
    }
}
