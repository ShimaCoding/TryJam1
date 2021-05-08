using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class pj_playerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public Rigidbody rb;
    Vector3 movement;
    public float attackRange = 3f;

    private Vector3 mousePos;
    Vector3 clickPosition;
    public LayerMask mouseLayer;
    public LayerMask entityLayer;
    public bool isFacingRight;
    public bool isFacingUp;
    public float dashDistance = 2f;

    Vector3 moveDirection;
    Vector3 lastMoveDir;
    public float pushEnemyForce = 500;

    public SkeletonAnimation spineSkel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //spineSkel.GetComponentInChildren<SkeletonAnimation>();
    }

    void Update()
    {
        GetAxis();

        RaycastHit hit;
        Ray rey = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rey, out hit, Mathf.Infinity, mouseLayer)) {

            clickPosition = hit.point;
            RaycastHit hit2;
            Ray ray = new Ray(transform.position, (clickPosition - transform.position).normalized);
            if (Physics.Raycast(ray, out hit2, attackRange, entityLayer)) {
                if (hit2.transform.CompareTag("Enemy")) {
                    if (Input.GetMouseButtonDown(1))
                        hit2.transform.GetComponent<mg_rewind>().RewindStart();
                    if (Input.GetMouseButtonDown(0))
                        PushEnemy(hit2.transform.GetComponent<Rigidbody>());
                        
                }
            }
        }
        Debug.DrawRay(transform.position, (clickPosition - transform.position).normalized * attackRange, Color.red);

        HandleDash();
    }

    private void GetAxis()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if(moveDirection.magnitude != 0)
            lastMoveDir = moveDirection;
        //Debug.Log(lastMoveDir.normalized + "last dir");
    }

    private void FixedUpdate()
    {
        if (movement.x == 1)
        {
            isFacingRight = true;
            FlipPlayer(isFacingRight);
        }
        else if (movement.x == -1)
        {
            isFacingRight = false;
            FlipPlayer(isFacingRight);
        }
        if (movement.z < 0)
        {
            isFacingUp = false;

        }
        else if (movement.z > 0)
        {
            isFacingUp = true;
        }
        moveDirection = movement.normalized;
        moveDirection = Quaternion.AngleAxis(-45, Vector3.up) * new Vector3(moveDirection.x * 0.7f, 0, moveDirection.z);
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    private void PushEnemy(Rigidbody enemyRb)
    {
        Vector3 direction = (enemyRb.transform.position - transform.position).normalized * pushEnemyForce;
        enemyRb.AddForce(direction);
        AudioManager.instance.Play("ForceStaff");
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += lastMoveDir * dashDistance;
            AudioManager.instance.Play("Teletransportation");
        }


    }

    void FlipPlayer(bool facing)
    {
        spineSkel.skeleton.FlipX = !facing;
    }

    public void Fall () {
        Collider[] cols = GetComponents<Collider>();
        foreach (Collider col in cols)
            col.enabled = false;
    }
}
