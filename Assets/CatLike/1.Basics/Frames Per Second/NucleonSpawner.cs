using UnityEngine;

namespace CatLike
{
    public class NucleonSpawner : MonoBehaviour
    {
        public float timeBetweenSpawns;
        public float spwanDistance;
        public Nucleon[] nucleons;

        private float timeSinceLastSpawn;

        private void FixedUpdate()
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= timeBetweenSpawns)
            {
                timeBetweenSpawns -= timeBetweenSpawns;
                SpawnNucleon();
            }
        }

        private void SpawnNucleon()
        {
            Nucleon prefb = nucleons[Random.Range(0, nucleons.Length)];
            Nucleon spawn = Instantiate<Nucleon>(prefb);
            spawn.transform.localPosition = Random.onUnitSphere * spwanDistance;
        }
    } 
}
