using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : OneTimeButton
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bear"))
        {
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bear"))
        {
            pressed.SetActive(false);
            unpressed.SetActive(true);
            if (linked != null)
            {
                for (int i = 0; i < linked.Length; ++i)
                {
                    linked[i].trigger(false);
                }
            }
            else
            {
                Debug.Log("null linked object");
            }
        }
    }

}
