using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;



// Sticks a spawner onto a GameObject so ECS doesn’t throw a fit
public class SummonAuthoring : MonoBehaviour
{
    public GameObject entityBlueprint; // The entity template (don’t forget to assign it or nothing happens)
    public float spawnSpeed; // How quickly entities pop into existence
}


// This class bakes the MonoBehaviour into an entity component
public class SummonBaker : Baker<SummonAuthoring>
{
    public override void Bake(SummonAuthoring magicCircle)
    {
        // Summons an entity (why does ECS make this so awkward?)
        Entity cursedObject = GetEntity(TransformUsageFlags.Dynamic);

        // Attaches the data package to the entity so it can function
        AddComponent(cursedObject, new Summoner
        {
            blueprint = GetEntity(magicCircle.entityBlueprint, TransformUsageFlags.Dynamic), // Actually assigns the correct prefab entity
            spawnPos = float2.zero, // Everything spawns at (0,0)
            spawnTick = 0, // Starts at 0 because why not
            howOften = magicCircle.spawnSpeed // How much time before it spits out another entity
        });
    }
}