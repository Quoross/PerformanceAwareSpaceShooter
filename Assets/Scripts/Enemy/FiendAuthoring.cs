using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Fiend
{
    public class FiendAuthoring : MonoBehaviour
    {
        public float3 Position; // Initial position of the fiend
        public float3 Velocity; // Movement velocity of the fiend
        public GameObject FiendPrefab; // Prefab reference for the fiend
        
        private class FiendAuthoringBaker : Baker<FiendAuthoring>
        {
            public override void Bake(FiendAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                // Adding fiend component data to the entity
                AddComponent(entity, new FiendComponents
                {
                    Position = authoring.Position,
                    Velocity = authoring.Velocity,
                    FiendPrefab = GetEntity(authoring.FiendPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    // Fiend component that stores position, velocity, and prefab reference
    public struct FiendComponents : IComponentData
    {
        public float3 Position;
        public float3 Velocity;
        public float SpawnYPosition; // Y-position where fiends spawn
        public float SpawnRate; // Interval at which fiends spawn
        public Entity FiendPrefab; // Prefab entity reference
    }
}