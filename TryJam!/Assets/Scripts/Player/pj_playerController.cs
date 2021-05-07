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
        //RaycastHit hit;
        //Ray ray;
        //ray = new Ray(transform.position, movement);
        //if (Physics.Raycast(ray, out hit, attackRange))
        //{

        //    if (hit.collider.tag == "Enemy")
        //    {
        //        print(hit.collider.name);
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            hit.transform.GetComponent<mg_rewind>().RewindStart();

        //        }

        //    }

        //}

        RaycastHit hit;
        Ray rey = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rey, out hit, Mathf.Infinity, layer))
        {
            if (hit.collider.tag == "Enemy")
            {
                print(hit.collider.name);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    hit.transform.GetComponent<mg_rewind>().RewindStart();

                }

            }
            clickPosition = hit.point;
            
        }
        Debug.DrawRay(transform.position, (clickPosition - transform.position).normalized * attackRange, Color.red);
        Debug.Log(clickPosition);

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);



    }
}
