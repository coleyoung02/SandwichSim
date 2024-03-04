using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueToTrigger;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueToTrigger.gameObject.SetActive(true);
            GameObject.Destroy(this.gameObject);
        }
    }
}
