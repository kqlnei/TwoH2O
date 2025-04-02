using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 5f; // ジャンプの高さ
    private bool isGrounded = true;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Move();
        Jump();
        ApplyGravity();
        transform.Translate(velocity * Time.deltaTime);
    }

    void Move()
    {
        float move = 0;
        if (Input.GetKey(KeyCode.D))
        {
            move = moveSpeed * Time.deltaTime;
        }

        transform.Translate(move, 0, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            velocity.y = CalculateJumpSpeed(jumpHeight);
            isGrounded = false;
        }
    }

    private float CalculateJumpSpeed(float jumpHeight)
    {
        return Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            velocity.y = 0; // 地面に着地したらy方向の速度をリセット
        }
    }
}
