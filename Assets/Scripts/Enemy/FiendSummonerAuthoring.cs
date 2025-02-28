using Unity.Entities;
using UnityEngine;

namespace Fiend.FiendSummoner
{
    public class FiendSummonerAuthoring : MonoBehaviour
    {
        public GameObject FiendPrefab; // Fiend prefab to be summoned
        public float SpawnRate; // Time interval between spawns
        public float SpawnHeight; // Y-position where fiends should spawn

        private class FiendSummonerAuthoringBaker : Baker<FiendSummonerAuthoring>
        {
            public override void Bake(FiendSummonerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                // Adding fiend component data to the entity
                AddComponent(entity, new FiendComponents
                {
                    FiendPrefab = GetEntity(authoring.FiendPrefab, TransformUsageFlags.Dynamic),
                    SpawnYPosition = authoring.SpawnHeight,
                    SpawnRate = authoring.SpawnRate,
                });

                // Debugging if prefab is missing
                if (authoring.FiendPrefab == null)
                {
                    Debug.LogError("FiendSummonerAuthoring: FiendPrefab is NULL!");
                }
                else
                {
                    Debug.Log($"FiendSummonerAuthoring: Assigned FiendPrefab {authoring.FiendPrefab.name}");
                }
            }
        }
    }
}