using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Fiend.FiendSummoner
{
    public partial struct FiendSummoner : ISystem
    {
        private NativeArray<Unity.Mathematics.Random> RandomArray;

        public void OnCreate(ref SystemState state)
        {
            // Ensure the system only runs when required
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            // Initialize random array for different threads
            RandomArray = new NativeArray<Unity.Mathematics.Random>(JobsUtility.MaxJobThreadCount, Allocator.Persistent);
            uint seed = (uint)System.Environment.TickCount;
            for (int i = 0; i < RandomArray.Length; i++)
            {
                RandomArray[i] = new Unity.Mathematics.Random(seed == 0 ? 1 : seed);
                seed++;
            }
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            double elapsedTime = SystemAPI.Time.ElapsedTime;
            float deltaTime = SystemAPI.Time.DeltaTime;

            // Loop through all entities with FiendComponents and summon new fiends
            foreach (var (fiendComponent, entity) in SystemAPI.Query<FiendComponents>().WithEntityAccess())
            {
                if (elapsedTime % fiendComponent.SpawnRate < deltaTime)
                {
                    int threadIndex = JobsUtility.ThreadIndex;
                    var random = RandomArray[threadIndex];
                    float randomX = random.NextFloat(-8f, 8f);
                    float randomScale = random.NextFloat(2f, 10f);

                    // Store updated random state
                    RandomArray[threadIndex] = random;

                    // Instantiate a new fiend entity
                    Entity newFiend = ecb.Instantiate(fiendComponent.FiendPrefab);
                    ecb.SetComponent(newFiend, LocalTransform.FromPositionRotationScale(
                        new float3(randomX, fiendComponent.SpawnYPosition, 0), 
                        quaternion.identity,
                        randomScale));
                }
            }

            // Execute all commands
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public void OnDestroy(ref SystemState state)
        {
            // Dispose of the random array to prevent memory leaks
            if (RandomArray.IsCreated)
                RandomArray.Dispose();
        }
    }
}
