﻿using UnityEngine;

public abstract class Tower : GameTileContent
{
    const int enemyLayerMask = 1 << 9;

    static Collider[] targetBuffer = new Collider[100];

    [SerializeField, Range(1.5f, 10.5f)] protected float targetingRange = 1.5f;

    public abstract TowerType TowerType { get; }

    protected bool AcquireTarget(out TargetPoint target)
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 3f;

        int hits = Physics.OverlapCapsuleNonAlloc(a, b, targetingRange, targetBuffer, enemyLayerMask);

        if (hits > 0)
        {
            target = targetBuffer[Random.Range(0, hits)].GetComponent<TargetPoint>();
            Debug.Assert(target != null, "Targeted non-enemy", targetBuffer[0]);
            return true;
        }
        target = null;
        return false;
    }
    protected bool TrackTarget(ref TargetPoint target)
    {
        if (target == null)
        {
            return false;
        }
        Vector3 a = transform.localPosition;
        Vector3 b = target.Position;
        float x = a.x - b.x;
        float z = a.z - b.z;
        float r = targetingRange + 0.125f * target.Enemy.Scale;
        if (x * x + z * z > r * r)
        {
            target = null;
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }
}