using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class en_patrolChancho : MonoBehaviour
{
    [SerializeField]
    bool patrolWaiting;
    [SerializeField]
    float totalWaitTime = 1f;
    [SerializeField]
    float switchProbability = 0.2f;
    float waitProbability = 0.2f;
    [SerializeField]
    List<en_wayPoint> patrolPoints;

    public NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool travelling;
    bool waiting;
    bool patrolFordward;
    float waitTimer;

    int waitingRandom;

    private void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent==null)
        {
            Debug.LogError("El NavMeshAgent no esta attachado a " +gameObject.name);
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Count >=2)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Cantidad de puntos de patruya insuficientes");
            }
        }
    }
    private void Update()
    {
        //Checkeamos si esta cerca del destino
        if (travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;
            navMeshAgent.speed = 4f;
            
            //Si es que vamos a esperar, esperamos
            if (patrolWaiting)
            {
                waiting = true;
                waitTimer = 0;
                
            }
            else
            {
                
                ChangePatrolPoint();
                SetDestination();
                

            }
        }
        //si estamos esperando
        if (waiting)
        {
            
            waitTimer += Time.deltaTime;
            if (waitTimer >= totalWaitTime)
            {
                
                waiting = false;
                patrolWaiting = false;
                ChangePatrolPoint();
                SetDestination();

            }
        }
    }

    private void SetDestination()
    {

        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
            if (UnityEngine.Random.Range(0f, 1f) <= waitProbability)
            {
                patrolWaiting = true;
            }

        }
        
    }

    private void ChangePatrolPoint()
    {

        if (UnityEngine.Random.Range(0f,1f) <= switchProbability)
        {
            patrolFordward = !patrolFordward;
        }
        if (patrolFordward)
        {
            currentPatrolIndex =  (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            if(--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }

    }
}
