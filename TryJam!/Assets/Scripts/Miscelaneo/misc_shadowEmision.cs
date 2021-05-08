using UnityEngine;

public class misc_shadowEmision : MonoBehaviour{

    public Transform target;

    private void Start () {
        transform.SetParent(null);
    }

    void Update() {
        if (target.position.y > 0.5f)
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        else
            transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
