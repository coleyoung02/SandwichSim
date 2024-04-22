using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BearController : MonoBehaviour, IPlayerInput
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpPower = 16f;

    [Header("Component References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject breakRight;
    [SerializeField] private GameObject bees;
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController pixel;
    [SerializeField] private RuntimeAnimatorController crisp;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] roars;
    [SerializeField] private AudioClip[] specialRoars;

    [Header("Controls")]
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode jumpKey = KeyCode.W;
    [SerializeField] private KeyCode breakKey = KeyCode.E;

    private GameInstanceManager instance;

    public void Start()
    {
        instance = Resources.FindObjectsOfTypeAll<GameInstanceManager>()[0];
        instance.UpdateGameState(GameState.Gameplay);
        if (GameInstanceManager.Instance.GetSugarLevel() < 4)
        {
            animator.runtimeAnimatorController = pixel;
        }
        else
        {
            animator.runtimeAnimatorController = crisp;
        }
    }

    private bool isFacingRight = true;
    private float movementX = 0f;

    public bool InputEnabled { get; set; }

    public BearController()
    {
        InputEnabled = true;
    }

    public void Update()
    {
        if (instance.isUnpaused())
        {
            Move();
            Jump();
            BreakDoor();
        }
        animator.SetFloat("Speed", Mathf.Abs(movementX));
    }

    public void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(movementX * moveSpeed, rigidBody.velocity.y);
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
    }

    public void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && IsGrounded() && InputEnabled)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
        }
        else if (Input.GetKeyUp(jumpKey) && InputEnabled)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        }
    }

    public void PlayNoise()
    {
        if (Random.Range(0, 5) == 0)
        {
            source.clip = specialRoars[Random.Range(0, specialRoars.Length)];
        }
        else
        {
            source.clip = roars[Random.Range(0, roars.Length)];
        }
        source.Play();
    }

    public void BreakDoor()
    {
        if (Input.GetKeyDown(breakKey) && !breakRight.activeSelf && beeDistance() < 1.1f)
        {
            animator.SetTrigger("Slam");
            PlayNoise();
            StartCoroutine(breakAndWait(breakRight));
        }
    }

    private float beeDistance()
    {
        return (gameObject.transform.position - bees.transform.position - new Vector3(0, .3f, 0)).magnitude;
    }
    IEnumerator breakAndWait(GameObject o)
    {
        o.SetActive(true);
        if (isFacingRight)
        {
            rigidBody.velocity = new Vector2(-1f, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(1f, rigidBody.velocity.y);
        }

        yield return new WaitForSeconds(1);
        o.SetActive(false);
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
