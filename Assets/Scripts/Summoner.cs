using Unity.Entities;
using Unity.Mathematics;

// A data sack that holds spawner info (literally just variables sitting around)
public struct Summoner : IComponentData
{
    public Entity blueprint; // The thing we want to spawn
    public float2 spawnPos; // For some reason, this is a 2D position despite the rest being 3D
    public float spawnTick; // Next time we do the thing
    public float howOften; // How quickly this factory pumps out entities
}