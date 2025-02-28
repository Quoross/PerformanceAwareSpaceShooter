using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using System.Collections.Generic;

// System that randomly decides when to throw entities into the world
public partial class EntitySummoner : SystemBase
{
    // Bucket for holding entities before they are let loose (not really needed but whatever)
    private List<(Entity thingToSpawn, float3 spawnSpot)> spawnBucket;

    // Runs once when the world decides this thing exists
    protected override void OnCreate()
    {
        spawnBucket = new List<(Entity, float3)>(); // Just here to collect garbage before we use it
    }

    // Runs every frame unless Unity decides to explode
    protected override void OnUpdate()
    {
        spawnBucket.Clear(); // Nukes the bucket every frame 

        // Searching for all spawner dudes in the game
        foreach (var spawnDude in SystemAPI.Query<RefRW<Summoner>>())
        {
            // If the spawner’s  timer says it’s time, we do the thing
            if (spawnDude.ValueRO.spawnTick < SystemAPI.Time.ElapsedTime)
            {
                float3 actualSpot = new float3(spawnDude.ValueRO.spawnPos.x, spawnDude.ValueRO.spawnPos.y, 0);
                
                // Add to the bucket because we like making things complicated
                spawnBucket.Add((spawnDude.ValueRO.blueprint, actualSpot));

                // Delay the next spawn attempt because spam is bad
                spawnDude.ValueRW.spawnTick = (float)(SystemAPI.Time.ElapsedTime + spawnDude.ValueRO.howOften);
            }
        }

        // Unleash the entities
        foreach (var (thingToSpawn, spawnSpot) in spawnBucket)
        {
            // Clone an entity from the void
            Entity chaosBeing = EntityManager.Instantiate(thingToSpawn);
            
            // Teleport it to the designated suffering area
            EntityManager.SetComponentData(chaosBeing, LocalTransform.FromPosition(spawnSpot));
        }
    }
}
