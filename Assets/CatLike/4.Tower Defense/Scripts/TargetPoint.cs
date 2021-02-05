using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; set; }
    public Vector3 Position => transform.position;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        Debug.Assert(Enemy != null, "Target point without Enemy root!", this);
        Debug.Assert(GetComponent<SphereCollider>() != null, "Traget point without sphere collider!", this);
        Debug.Assert(gameObject.layer == 9, "Traget point on wrong layer!", this);
    }
}
