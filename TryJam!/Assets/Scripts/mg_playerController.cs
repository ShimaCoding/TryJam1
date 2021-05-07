using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_playerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public Rigidbody rb;
    Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetAxis();
    }

    private void GetAxis()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
