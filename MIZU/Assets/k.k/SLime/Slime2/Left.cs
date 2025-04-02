using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 5f; // ÉWÉÉÉìÉvÇÃçÇÇ≥
    private Rigidbody rb;
    private bool isGrounded = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float move = 0;
        
        if (Input.GetKey(KeyCode.A))
        {
            move = -moveSpeed * Time.deltaTime;
        }
        transform.Translate(move, 0, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, CalculateJumpSpeed(jumpHeight), rb.velocity.z);
            isGrounded = false;
        }
    }

    private float CalculateJumpSpeed(float jumpHeight)
    {
        return Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
