using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class en_enemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float agentSpeed, agentAcceleration;
    public float pushSpeed;

    //patroll
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float timeBetweenAttacks = 2;
    bool alreadyAttacked = false;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //Checkea si el agente esta viendo y esta en rango
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) Attack();
        if (Mathf.Abs(transform.position.x) > 9.1 || Mathf.Abs(transform.position.z) > 9.1 && agent.GetComponent<Rigidbody>().velocity.magnitude > pushSpeed)
        {
            agent.enabled = false;

            if (transform.position.x > 8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.right * 20);
            }
            else if (transform.position.x < -8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.left * 20);
            }
            else if (transform.position.z < -8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.back * 20);
            }
            else if (transform.position.z > 8.9)
            {
                agent.GetComponent<Rigidbody>().AddForce(Vector3.forward * 20);
            }
        }


    }


    private void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();
        if (agent != null) 
        {
            if (walkPointSet) agent.SetDestination(walkPoint);
        }
        

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        agent.speed = 3.5f;

    }
    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, 1, +transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {

        if (Vector3.Distance(player.position, transform.position) > 4)
        {
            if (transform.rotation.x <= 1)
            {
                transform.LookAt(player);

            }

        }

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            StartCoroutine(Charge(player.position));
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    IEnumerator Charge(Vector3 direction)
    {
        float distance = Vector3.Distance(player.position, direction);
        agent.isStopped = true;
        yield return new WaitForSeconds(.4f);
        agent.isStopped = false;
        agent.SetDestination(direction);
        agent.speed = agentSpeed;
        agent.acceleration = agentAcceleration;
    }


    public void CancelCharge()
    {
        agent.isStopped = true;
    }
}
