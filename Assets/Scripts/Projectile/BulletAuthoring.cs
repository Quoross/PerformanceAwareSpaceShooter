using Unity.Entities;
using UnityEngine;

public class ThingThatShootsAuthoring : MonoBehaviour
{
    public float SpeedOfProjectile; // Speed of bullets
}

public class ThingThatShootsBaker : Baker<ThingThatShootsAuthoring>
{
    public override void Bake(ThingThatShootsAuthoring someScriptableComponent)
    {
        Entity ent = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(ent, new ProjectileMoveSpeed
        {
            Value = someScriptableComponent.SpeedOfProjectile
        });
    }
}