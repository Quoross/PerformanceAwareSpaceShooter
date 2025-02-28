using Unity.Entities;
using Unity.Transforms;
using UnityEngine; // For Debug.Log

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootThingSystem : ISystem
{
    public void OnUpdate(ref SystemState st8)
    {
        var bufferOfCommandsForEntities = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        foreach (var (blueprintProjectile, thingTransform) in SystemAPI.Query<ProjectilePrefab, LocalTransform>().WithAll<FireProjectileTag>())
        {
            var spawnedProjectile = bufferOfCommandsForEntities.Instantiate(blueprintProjectile.Value);

            var newProjTransform = LocalTransform.FromPositionRotation(thingTransform.Position, thingTransform.Rotation);
            bufferOfCommandsForEntities.SetComponent(spawnedProjectile, newProjTransform);

            // FIX: Ensure projectiles have a movement speed
            bufferOfCommandsForEntities.AddComponent(spawnedProjectile, new ProjectileMoveSpeed { Value = 10f });

            Debug.Log("Projectile Spawned!"); // Debug to confirm spawning
        }

        bufferOfCommandsForEntities.Playback(st8.EntityManager);
        bufferOfCommandsForEntities.Dispose();
    }
}