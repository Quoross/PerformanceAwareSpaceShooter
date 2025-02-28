using Unity.Entities;
using Unity.Mathematics;

// Component that lets the player move 
public struct PlayerComponent : IComponentData
{
    public float Speed; // The speed at which the player moves
}

// Stores the movement input from the player
public struct PlayerMoveInput : IComponentData
{
    public float2 Value; // X and Y input (basically just a vector, but cooler)
}

// Stores how fast the player moves
public struct PlayerMoveSpeed : IComponentData
{
    public float Value; // Speed of movement, multiplied every frame
}

// Empty component to mark an entity as "the player"
public struct PlayerTag : IComponentData { }

// Holds a reference to the bullet prefab (so we can shoot things)
public struct ProjectilePrefab : IComponentData
{
    public Entity Value; // The bullet we spawn when the player gets trigger-happy
}

// Determines how fast projectiles go (because slow bullets are lame)
public struct ProjectileMoveSpeed : IComponentData
{
    public float Value; // Speed of the bullet (bigger = faster)
}

// Tag that tells us whether the player is currently firing
public struct FireProjectileTag : IComponentData, IEnableableComponent { } // Shooting mode: ON/OFF