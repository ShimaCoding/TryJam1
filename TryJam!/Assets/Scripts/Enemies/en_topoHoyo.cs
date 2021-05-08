using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_topoHoyo : MonoBehaviour {

    float disappearTimer = 5f;

    Collider hoyoCol;

    void Start() {
        hoyoCol = GetComponent<Collider>();
        Invoke("ActivateCol", 2.8f);
    }

    void ActivateCol () {
        hoyoCol.enabled = true;
    }

    void Update() {
        disappearTimer -= Time.deltaTime;
        if(disappearTimer <= 0) {
            if (hoyoCol.enabled)
                hoyoCol.enabled = false;
            if (transform.localScale.x > 0)
                transform.localScale -= Vector3.one * 4 * Time.deltaTime;
            else Destroy(gameObject);
        }
    }

    private void OnTriggerEnter (Collider other) {
        if((other.CompareTag("Player") || other.CompareTag("Enemy"))) {
            other.gameObject.SendMessage("Fall", SendMessageOptions.DontRequireReceiver);
        }
    }
}
