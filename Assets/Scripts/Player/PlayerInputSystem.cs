using Unity.Entities;
using UnityEngine; 

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class PlayerInputSystem : SystemBase
{
    private Entity playableDude; // The player's entity

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>(); // We refuse to run unless a player exists
        RequireForUpdate<PlayerMoveInput>(); // Same for movement input
        Debug.Log("PlayerInputSystem initialized!"); 
    }

    protected override void OnStartRunning()
    {
        playableDude = SystemAPI.GetSingletonEntity<PlayerTag>(); // Find the player entity
        Debug.Log("Player entity linked in PlayerInputSystem!"); 
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.Exists(playableDude)) 
        {
            Debug.LogWarning("Player entity vanished!"); 
            return;
        }

        // Read input axes (WASD or arrow keys)
        float leftRight = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float upDown = Input.GetAxis("Vertical"); // W/S or Up/Down
        Vector2 movementCommand = new Vector2(leftRight, upDown);

        // Actually update the player's movement input
        SystemAPI.SetSingleton(new PlayerMoveInput { Value = movementCommand });

        // Shooting logic: Press Space to fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pew! Pew! (Firing enabled)"); // Sound effects included(not actually)
            SystemAPI.SetComponentEnabled<FireProjectileTag>(playableDude, true);
        }
        else
        {
            SystemAPI.SetComponentEnabled<FireProjectileTag>(playableDude, false);
        }
    }

    protected override void OnStopRunning()
    {
        Debug.Log("PlayerInputSystem stopping! Player set to null.");
        playableDude = Entity.Null;
    }
}
