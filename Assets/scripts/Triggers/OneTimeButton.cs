using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeButton : MonoBehaviour
{
    [SerializeField] protected GameObject unpressed;
    [SerializeField] protected GameObject pressed;
    [SerializeField] protected Interactable[] linked;
    private bool down = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!down && collision.gameObject.layer == LayerMask.NameToLayer("Bear"))
        {
            down = true;
            unpressed.SetActive(false);
            pressed.SetActive(true);
            if (linked != null)
            {
                for (int i = 0; i < linked.Length; ++i)
                {
                    linked[i].trigger(true);
                }
            }
            else
            {
                Debug.Log("null linked object");
            }
        }
    }
}
