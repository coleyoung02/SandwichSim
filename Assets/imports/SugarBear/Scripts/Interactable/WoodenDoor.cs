using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject col;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "DoorBreak")
        {
            animator.SetTrigger("Break");
            col.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "DoorBreak")
        {
            animator.SetTrigger("Break");
            col.SetActive(false);
        }
    }
}
