using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float slowingRate = 20f;
    [SerializeField] private float maxDist = 400f;
    [SerializeField] private float moveSpeed = 50f;
    private bool stopping;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        stopping = false;
        rb.velocity = moveSpeed * transform.right;
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Frobbable.hasLayer(LayerMask.GetMask("Player"), collision.gameObject.layer))
            stopping = true;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > maxDist)
        {
            Destroy(gameObject);
        }
        if (rb.velocity.magnitude > 0 && stopping)
        {
            float newVmag = Mathf.Max(0, rb.velocity.magnitude - Time.deltaTime * slowingRate * Mathf.Sign(moveSpeed));
            rb.velocity = rb.velocity.normalized * newVmag;
        }
    }

    public int GetSign()
    {
        return Mathf.RoundToInt( Mathf.Sign(moveSpeed));
    }
}