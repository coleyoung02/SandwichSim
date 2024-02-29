using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Dialogue")]
    [SerializeField] private float typeDelay = 0.02f;
    [TextArea(3, 10)]
    [SerializeField] private string[] spokenLines;

    private int index = 0;
    private string currentLine = "";

    private void Start()
    {
        StartDialogue();
    }

    public void Update()
    {
        if (dialogueBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (textDisplay.text == currentLine)
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textDisplay.text = currentLine;
                }
            }
        }
    }

    public void StartDialogue()
    {
        UpdatePlayerInput(false);
        dialogueBox.SetActive(true);
        textDisplay.text = "";
        currentLine = spokenLines[index];
        StartCoroutine(Type());
    }

    public void UpdatePlayerInput(bool newVal)
    {
        GameObject[] playerCharacters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject character in playerCharacters)
        {
            if (character != null)
            {
                character.GetComponent<IPlayerInput>().InputEnabled = newVal;
            }
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in currentLine.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeDelay);
        }
    }

    public void NextLine()
    {
        index++;

        if (index < spokenLines.Length)
        {
            textDisplay.text = "";
            currentLine = spokenLines[index];
            StartCoroutine(Type());
        }
        else
        {
            FinishDialogue();
        }
    }

    public void FinishDialogue()
    {
        textDisplay.text = "";
        dialogueBox.SetActive(false);
        UpdatePlayerInput(true);
        GameObject.Destroy(this);
    }
}
