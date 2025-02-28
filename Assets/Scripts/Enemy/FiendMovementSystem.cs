using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Fiend
{
    [BurstCompile]
    public partial class FiendMovementSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            // Loop through all entities with FiendComponents and update position
            Entities.ForEach((ref LocalTransform transform, in FiendComponents movement) =>
            {
                transform.Position += movement.Velocity * deltaTime;
            }).ScheduleParallel();
        }
    }
}