using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine; 

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState mysteriousState)
    {
        Camera eyeball = Camera.main; // We need this to keep the player inside the screen
        float tinyTimeStep = SystemAPI.Time.DeltaTime; // Time since last frame

        if (eyeball != null) // If we somehow don't have a camera, we abandon ship
        {
            float3 minWall = eyeball.ScreenToWorldPoint(new Vector3(0, 0, 0)); // Bottom-left corner
            float3 maxWall = eyeball.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // Top-right corner

            foreach (var (playerLocation, movementInput, speedOfZooming) in SystemAPI.Query<RefRW<LocalTransform>, PlayerMoveInput, PlayerMoveSpeed>())
            {
                // Calculate new position based on input and speed
                float2 newSpot = playerLocation.ValueRW.Position.xy + movementInput.Value * speedOfZooming.Value * tinyTimeStep;
                
                // Make sure we don't let the player escape
                newSpot = math.clamp(newSpot, minWall.xy, maxWall.xy);
                playerLocation.ValueRW.Position.xy = newSpot;

                Debug.Log($"Player moved to: {newSpot}");
            }
        }
        else
        {
            Debug.LogError("Camera is missing! The player is lost in the void.");
        }
    }
}