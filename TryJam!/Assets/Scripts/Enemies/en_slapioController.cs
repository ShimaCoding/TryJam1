﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_slapioController : MonoBehaviour {

    float arenaLimit = 8.5f;
    float speed = 10f;
    float turnTimer = 0.2f;
    float slapioForce = 1000f;
    bool move;

    Vector3 direction = Vector3.forward;

    public GameObject bodyObj;
    List<GameObject> collidedObjs = new List<GameObject>();

    void Start() {
        transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
        StartCoroutine("Appear");
    }

    void Update() {
        if (move) {
            transform.Translate(direction * speed * Time.deltaTime);
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0) {
                bool directionAxis = (Random.value > 0.5f);
                if (directionAxis) {
                    float newZ = Random.Range(-1f, 1f);
                    direction = new Vector3(0, 0, newZ).normalized;
                }
                else {
                    float newX = Random.Range(-1f, 1f);
                    direction = new Vector3(newX, 0, 0).normalized;
                }

                turnTimer = Random.Range(0.2f, 0.7f);
                bodyObj.transform.forward = direction;
                speed = Random.Range(7f, 15f);
            }

            if(Mathf.Abs(transform.position.x) >= arenaLimit) {
                float newZ = Random.Range(-1f, 1f);
                if(transform.position.x > 0)
                    direction = new Vector3(-1, 0, newZ).normalized;
                else direction = new Vector3(1, 0, newZ).normalized;
                bodyObj.transform.forward = direction;
                speed = Random.Range(7f, 15f);
            }
            else if(Mathf.Abs(transform.position.z) >= arenaLimit) {
                float newX = Random.Range(-1f, 1f);
                if(transform.position.z > 0)
                    direction = new Vector3(newX, 0, -1).normalized;
                else direction = new Vector3(newX, 0, 1).normalized;
                bodyObj.transform.forward = direction;
                speed = Random.Range(7f, 15f);
            }
        }
    }

    IEnumerator Appear () {
        yield return new WaitForSeconds(2);
        while(transform.position.y < 0.5f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0.6f, transform.position.z), 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        yield return new WaitForSeconds(0.5f);
        move = true;
    }

    IEnumerator UltraSlapio () {
        move = false;
        yield return new WaitForSeconds(0.5f);
        if (collidedObjs.Count > 0)
            foreach (GameObject obj in collidedObjs) {
                Vector3 slapioVector = (obj.transform.position - transform.position).normalized * slapioForce;
                slapioVector.y = 0.5f * slapioForce;
                obj.GetComponent<Rigidbody>().AddForce(slapioVector);
            }
        yield return new WaitForSeconds(0.5f);
        move = true;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            collidedObjs.Add(other.gameObject);
    }
    private void OnTriggerExit (Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            collidedObjs.Remove(other.gameObject);
    }
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            StartCoroutine("UltraSlapio");
    }
}