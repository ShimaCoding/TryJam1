using UnityEngine;

public class misc_shadowEmision : MonoBehaviour{

    public Transform target;

    private void Start () {
        transform.SetParent(null);
    }

    void Update() {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
