using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_slapioController : MonoBehaviour {

    float arenaLimit = 8.5f;
    float speed = 4f;
    float turnTimer = 0.5f;
    float disappearTimer = 7f;
    [HideInInspector]
    public float slapioForce = 1000f;
    bool move;
    bool ascended;

    Vector3 direction = Vector3.forward;

    public GameObject bodyObj;
    List<GameObject> collidedObjs = new List<GameObject>();
    mg_rewind rewind;

    void Start() {
        rewind = GetComponent<mg_rewind>();
        transform.position = new Vector3(transform.position.x, -4f, transform.position.z);
        //StartCoroutine("Appear");
    }

    void Update() {
        if (!ascended) {
            disappearTimer -= Time.deltaTime;
            if(disappearTimer <= 0) {
                StartCoroutine("Appear");
                disappearTimer = Random.Range(10f, 15f);
            }
            return;
        }
        disappearTimer -= Time.deltaTime;
        if(disappearTimer <= 0) {
            StartCoroutine("Disappear");
            disappearTimer = Random.Range(10f, 15f);
            return;
        }
        if (move) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

                turnTimer = Random.Range(0.5f, 2.5f);
                transform.forward = direction;
                speed = Random.Range(2f, 4f);
            }

            if(Mathf.Abs(transform.position.x) >= arenaLimit) {
                float newZ = Random.Range(-1f, 1f);
                if(transform.position.x > 0)
                    direction = new Vector3(-1, 0, newZ).normalized;
                else direction = new Vector3(1, 0, newZ).normalized;
                transform.forward = direction;
                speed = Random.Range(2f, 4f);
            }
            else if(Mathf.Abs(transform.position.z) >= arenaLimit) {
                float newX = Random.Range(-1f, 1f);
                if(transform.position.z > 0)
                    direction = new Vector3(newX, 0, -1).normalized;
                else direction = new Vector3(newX, 0, 1).normalized;
                transform.forward = direction;
                speed = Random.Range(2f, 4f);
            }
        }
    }

    IEnumerator Appear () {
        transform.position = new Vector3(Random.Range(-arenaLimit, arenaLimit), -4, Random.Range(-arenaLimit, arenaLimit));
        yield return new WaitForSeconds(2);
        while(transform.position.y < 0.5f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0.6f, transform.position.z), 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        yield return new WaitForSeconds(0.5f);
        move = true;
        ascended = true;
    }
    IEnumerator Disappear () {
        ascended = false;
        move = false;
        yield return new WaitForSeconds(1);
        while (transform.position.y > -4f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -4.3f, transform.position.z), 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator UltraSlapio () {
        move = false;
        yield return new WaitForSeconds(0.5f);
        rewind.AddAction("ShadowSlapio");
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
        if (move && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")))
            StartCoroutine("UltraSlapio");
    }
}
