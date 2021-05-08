using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_entityDestroyer : MonoBehaviour
{
    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Enemy"))
            Destroy(other);
    }
}
