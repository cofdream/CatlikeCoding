using UnityEngine;

namespace CatLike
{
    public abstract class SpawnZone : MonoBehaviour
    {
        public abstract Vector3 SpawnPoint { get; }

    }

}