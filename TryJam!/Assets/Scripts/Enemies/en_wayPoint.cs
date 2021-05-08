using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_wayPoint : MonoBehaviour
{
    protected float debugDrawRadius = .5f;
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
