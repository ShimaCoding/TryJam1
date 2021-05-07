using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_slapioShadow : MonoBehaviour {

    float slapioForce = 1000f;

    List<GameObject> collidedObjs = new List<GameObject>();
    public en_slapioController slapioMainScript;

    void Start() {
        slapioForce = slapioMainScript.slapioForce;
    }

    public void ShadowSlapio () {
        StartCoroutine("UltraSlapio");
    }

    IEnumerator UltraSlapio () {
        if (collidedObjs.Count > 0)
            foreach (GameObject obj in collidedObjs) {
                Vector3 slapioVector = (obj.transform.position - transform.position).normalized * slapioForce;
                slapioVector.y = 0.5f * slapioForce;
                obj.GetComponent<Rigidbody>().AddForce(slapioVector);
            }
        yield return null;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            collidedObjs.Add(other.gameObject);
    }
    private void OnTriggerExit (Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            collidedObjs.Remove(other.gameObject);
    }
}
