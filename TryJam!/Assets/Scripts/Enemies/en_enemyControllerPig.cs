using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class en_enemyControllerPig : MonoBehaviour
{
    Rigidbody rb;
    public float speedMove = 5;
    public float dist = 10;
    public float attackRange = 7f;
    public bool isCharging;
    public float pigForce;
    public NavMeshAgent agent;
    public float pushSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        Charge();
        //print(rb.velocity.magnitude + " Velocidad Chancho");
        if (Mathf.Abs(transform.position.x) > 9 || Mathf.Abs(transform.position.z) > 9 && agent.GetComponent<Rigidbody>().velocity.magnitude > pushSpeed)
        {
            agent.enabled = false;

            if (transform.position.x > 8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.right * 35);
            }
            else if (transform.position.x < -8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.left * 35);
            }
            else if (transform.position.z < -8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.back * 35);
            }
            else if (transform.position.z > 8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.forward * 35);
            }
        }

    }

    private void Charge()
    {
        RaycastHit hit;
        Vector3 lastPosition;
        Ray landingRay = new Ray(transform.position, transform.forward * attackRange);
        Debug.DrawRay(transform.position,transform.forward * attackRange, Color.green);
        if (Physics.Raycast(landingRay, out hit, dist))
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
            {
                //Obtengo la posicion del player
                lastPosition = hit.collider.transform.position;
                //Insertar Logica de ejecucion de la carga del chancho
                Debug.Log("Realizar Carga!!!!");
                isCharging = true;
                GetComponent<en_patrolChancho>().navMeshAgent.speed = 10f;
                
                if (Vector3.Distance(transform.position,hit.transform.position) < 3)
                {
                    Vector3 chargeVector = (transform.forward).normalized * pigForce;
                    chargeVector.y = 1.7f * pigForce;
                    hit.collider.GetComponent<Rigidbody>().AddForce(chargeVector);
                    print(hit.collider.name);
                    AudioManager.instance.Play("LoudPig");
                }
                
                
                //MoveTo(lastPosition);
                
            }

        }
    }

    public void MoveTo(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) >= 2)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, position, speedMove * Time.fixedDeltaTime));
        }

    }

    public void Fall () {
        agent.enabled = false;
        Collider[] cols = GetComponents<Collider>();
        foreach (Collider col in cols)
            col.enabled = false;
    }
}
