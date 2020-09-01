using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLike
{
    public class GameLevel : MonoBehaviour
    {
        [SerializeField] SpawnZone spawnZone = null;

        private void Start()
        {
            Game.Instance.SpawnZoneOfLevel = spawnZone;
        }
    }

}