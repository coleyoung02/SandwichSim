using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeeController : MonoBehaviour, IPlayerInput
{
    [Header("Movement")]
    [SerializeField] private float flyHorizontalSpeed = 8f;
    [SerializeField] private float flyVerticalSpeed = 8f;

    [Header("Component References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController pixel;
    [SerializeField] private RuntimeAnimatorController cloud;
    [SerializeField] private RuntimeAnimatorController bruh;
    [SerializeField] private AudioSource source;

    [Header("Controls")]
    [SerializeField] private KeyCode moveRightKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode moveUpKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode moveDownKey = KeyCode.DownArrow;

    private GameInstanceManager instance;

    public void Start()
    {
        instance = Resources.FindObjectsOfTypeAll<GameInstanceManager>()[0];
        if (SceneManager.GetActiveScene().buildIndex < 5)
        {
            animator.runtimeAnimatorController = pixel;
        }
        else if(SceneManager.GetActiveScene().buildIndex < 7)
        {
            animator.runtimeAnimatorController = cloud;
        }
        else
        {
            animator.runtimeAnimatorController = bruh;
        }
    }

    private bool isFacingRight = true;
    private float movementX = 0f;
    private float movementY = 0f;

    public bool InputEnabled { get; set; }

    public BeeController()
    {
        InputEnabled = true;
    }

    public void Update()
    {
        
        if (instance.isUnpaused())
        {
            Move();
        }
        if (movementX == 0 && movementY == 0)
        {
            source.volume = .02f;
        }
        else
        {
            source.volume = .15f;
        }
    }

    public void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(movementX * flyHorizontalSpeed, movementY * flyVerticalSpeed);
    }

    public void Move()
    {
        if (Input.GetKey(moveRightKey) && InputEnabled)
        {
            movementX = 1f;
            if (!isFacingRight)
            {
                FlipSprite();
            }
        }
        else if (Input.GetKey(moveLeftKey) && InputEnabled)
        {
            movementX = -1f;
            if (isFacingRight)
            {
                FlipSprite();
            }
        }
        else
        {
            movementX = 0f;
        }

        if (Input.GetKey(moveUpKey) && InputEnabled)
        {
            movementY = 1f;
        }
        else if (Input.GetKey(moveDownKey) && InputEnabled)
        {
            movementY = -1f;
        }
        else
        {
            movementY = 0f;
        }
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
