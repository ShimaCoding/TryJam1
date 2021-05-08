using UnityEngine;

public class misc_shadowEmision : MonoBehaviour{

    public Transform target;

    private void Start () {
        transform.SetParent(null);
    }

    void Update() {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.rotation = target.rotation;
    }
}
