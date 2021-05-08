using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_topoController : MonoBehaviour {

    float arenaRange = 8f;
    float reacsendTimer = 5;
    float slapioForce = 500f;
    bool ascending;
    bool asomandose;

    public GameObject agujeroPref;
    GameObject lastAgujero;

    private void Start () {
        transform.position = new Vector3(0, -3, 0);
    }

    void Update() {
        if (ascending)
            return;
        reacsendTimer -= Time.deltaTime;
        if(reacsendTimer <= 0) {
            StartCoroutine("Ascend");
            reacsendTimer = Random.Range(5f, 10f);
        }
    }

    IEnumerator Ascend () {
        ascending = true;
        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        transform.position = new Vector3(Random.Range(-arenaRange, arenaRange), -3, Random.Range(-arenaRange, arenaRange));
        GameObject newAgujeroObj = Instantiate(agujeroPref, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        lastAgujero = newAgujeroObj;
        newAgujeroObj.transform.localScale = Vector3.zero;
        while(newAgujeroObj.transform.localScale.magnitude < 1) {
            newAgujeroObj.transform.localScale += Vector3.one * 5 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newAgujeroObj.transform.localScale = Vector3.one;
        asomandose = true;
        while (transform.position.y < 0) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0.1f, transform.position.z), 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        asomandose = false;
        yield return new WaitForSeconds(1);
        ascending = false;
        StartCoroutine("Descend");
    }

    IEnumerator Descend () {
        while (lastAgujero.transform.localScale.magnitude < 2.5f) {
            lastAgujero.transform.localScale += Vector3.one * 7 * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -3.3f, transform.position.z), 3.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        while (transform.position.y > -3) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -3.3f, transform.position.z), 3.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (asomandose && (other.CompareTag("Player") || other.CompareTag("Enemy"))) {
            Vector3 slapioVector = (other.transform.position - transform.position).normalized * slapioForce;
            slapioVector.y *= 3;
            other.GetComponent<Rigidbody>().AddForce(slapioVector);
        }
    }
}
