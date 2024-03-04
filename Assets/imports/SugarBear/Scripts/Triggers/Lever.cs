using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyCode interactKey = KeyCode.RightShift;
    [SerializeField] private GameObject unflipped;
    [SerializeField] private GameObject flipped;
    [SerializeField] private Interactable[] linked;
    [SerializeField] private Animator animator;
    private bool isFlipped;
    private Collider2D col;

    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
        isFlipped = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("IsIn", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("IsIn", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (col.IsTouchingLayers(LayerMask.GetMask("Bees")))
            {
                animator.SetTrigger("Flip");
                isFlipped = !isFlipped;
                //unflipped.SetActive(!isFlipped);
                //flipped.SetActive(isFlipped);
                if (linked != null)
                {
                    for (int i = 0; i < linked.Length; ++i)
                    {
                        linked[i].trigger(isFlipped);
                    }
                }
                else
                {
                    Debug.Log("null linked object");
                }
            }
        }
    }


}
