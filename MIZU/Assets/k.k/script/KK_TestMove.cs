using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KK_TestMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveHorizontal = 0;
        float moveVertical = 0;

        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveVertical = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -moveSpeed * Time.deltaTime;
        }

        transform.Translate(moveHorizontal, moveVertical, 0);
    }
}
