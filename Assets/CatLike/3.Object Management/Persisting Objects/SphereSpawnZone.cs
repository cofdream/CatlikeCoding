using UnityEngine;

namespace CatLike
{
    public class SphereSpawnZone : SpawnZone
    {
        public override Vector3 SpawnPoint => transform.TransformPoint(surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);

        [SerializeField] private bool surfaceOnly = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
    }

}