using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pj_playerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public Rigidbody rb;
    Vector3 movement;
    public float attackRange = 3f;

    private Vector3 mousePos;
    Vector3 clickPosition;
    public LayerMask layer;


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

        RaycastHit hit;
        RaycastHit hit2;
        Ray rey = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(rey, out hit, Mathf.Infinity, layer))
        {

            clickPosition = hit.point;
            Ray ray = new Ray(transform.position, (clickPosition - transform.position).normalized * attackRange);
            if (Physics.Raycast(ray, out hit2,attackRange))
            {
                if (hit2.collider.tag == "Enemy")
                {
                    print(hit2.collider.name);
                    if (Input.GetMouseButtonDown(1))
                    {
                        print("right mouse pressed");
                        hit2.transform.GetComponent<mg_rewind>().RewindStart();

                    }

                }
            }

        }
        Debug.DrawRay(transform.position, (clickPosition - transform.position).normalized * attackRange, Color.red);

        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}
