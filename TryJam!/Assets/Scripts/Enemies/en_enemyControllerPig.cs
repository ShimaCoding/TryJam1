using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_enemyControllerPig : MonoBehaviour
{
    Rigidbody rb;
    public float speedMove = 5;
    public float dist = 10;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        RaycastHit hit;
        Vector3 lastPosition;
        Ray landingRay = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(landingRay, out hit, dist))
        {
            if (hit.collider.tag == "Player")
            {
                //Obtengo la posicion del player
                lastPosition = hit.collider.transform.position;
                //Insertar Logica de ejecucion de la carga del chancho
                Debug.Log("Realizar Carga!!!!");
                MoveTo(lastPosition);
            }
           
        }
        
    }
    private void MoveTo(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) >= 2)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, position, speedMove * Time.fixedDeltaTime));
        }

    }
}
