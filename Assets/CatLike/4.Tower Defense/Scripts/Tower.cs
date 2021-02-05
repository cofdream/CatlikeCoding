﻿using UnityEngine;

public class Tower : GameTileContent
{
    const int enemyLayerMask = 1 << 9;

    static Collider[] targetBuffer = new Collider[100];

    [SerializeField, Range(1.5f, 10.5f)] float targetingRange = 1.5f;
    TargetPoint target;

    [SerializeField] Transform turret = default;
    [SerializeField] Transform laserBeam = default;
    [SerializeField, Range(1f, 100f)] float damage = 10f;

    Vector3 laserBeamScale;
    private void Awake()
    {
        laserBeamScale = laserBeam.localScale;
    }
    public override void GameUpdate()
    {
        if (TrackTarget() || AcquireTarget())
        {
            Shoot();
        }
        else
        {
            laserBeam.localScale = Vector3.zero;
        }
        Debug.Log("Searching for target...");
    }
    private void Shoot()
    {
        Vector3 point = target.Position;
        turret.LookAt(point);
        laserBeam.localRotation = turret.localRotation;

        float d = Vector3.Distance(turret.position, point);
        laserBeamScale.z = d;
        laserBeam.localScale = laserBeamScale;
        laserBeam.localPosition = turret.localPosition + 0.5f * d * laserBeam.forward;

        target.Enemy.ApplyDamage(damage * Time.deltaTime);
    }
    bool AcquireTarget()
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
    bool TrackTarget()
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
        if (target != null)
        {
            Gizmos.DrawLine(position, target.Position);
        }
    }
}