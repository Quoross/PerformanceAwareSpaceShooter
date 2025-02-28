using Unity.Entities;
using UnityEngine;


public class PlayerAuthoring : MonoBehaviour
{
    public GameObject ProjectilePrefab; // Assign this in the editor or we cry
    public float MoveSpeed = 5f; // Default speed (because balance is for later)
}

// This is where we make the player actually work in ECS
public class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity theChosenOne = GetEntity(TransformUsageFlags.Dynamic); // The player entity
        
        AddComponent<PlayerTag>(theChosenOne); // Now it's officially "the player"
        AddComponent<PlayerMoveInput>(theChosenOne); // Movement data goes here
        
        AddComponent(theChosenOne, new PlayerMoveSpeed
        {
            Value = authoring.MoveSpeed // Take speed from the MonoBehaviour
        });

        AddComponent<FireProjectileTag>(theChosenOne); // Default shooting behavior
        SetComponentEnabled<FireProjectileTag>(theChosenOne, false); // Start with "not shooting"

        AddComponent(theChosenOne, new ProjectilePrefab
        {
            Value = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic) // Assign projectile entity
        });

        Debug.Log("Player baked into an entity! Hope it doesn't burn."); // Confirmation log
    }
}