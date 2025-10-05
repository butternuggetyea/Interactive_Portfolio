using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
partial struct CubeSpawnerSystem : ISystem
{
  
    public void OnCreate(ref SystemState state)
    {
        
        
   
    }



    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        int count = 0;
        foreach (RefRW<CubeLogic> _cube in SystemAPI.Query<RefRW<CubeLogic>>()) 
        {
            count++;
        }

        foreach (RefRW<CubeSpawner> _spawner in SystemAPI.Query<RefRW<CubeSpawner>>())
        {
            if (count < _spawner.ValueRO.Count())
            {
                for (int i = 0; i < _spawner.ValueRO.Count() - count; i++)
                {
                    float r = _spawner.ValueRO._distance * math.pow(Random.value, 1.0f / 3.0f);
                    float3 position = GetRandomPositionInSphere(_spawner.ValueRO._spawnCenter, r);


                    Entity tmp = ecb.Instantiate(_spawner.ValueRW._cube);
                    ecb.SetComponent<CubeLogic>(tmp, new CubeLogic
                    {
                        _center = _spawner.ValueRO._spawnCenter,
                        _distance = r
                    });
                    ecb.SetComponent<LocalTransform>(tmp, new LocalTransform
                    {
                        Position = position,
                        Scale = 1f,
                        Rotation = quaternion.identity
                    });

                }
            }

            if (count > _spawner.ValueRO.Count()) 
            {
                int tmp = count - _spawner.ValueRO.Count();

                foreach ((RefRW<CubeLogic> _cube, Entity entity ) in SystemAPI.Query<RefRW<CubeLogic>>().WithEntityAccess())
                {
                    if (tmp > 0)
                    {
                        ecb.DestroyEntity(entity);
                        tmp--;
                    }
                }
            }

        }


        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    private float3 GetRandomPositionInSphere(float3 center, float r)
    {
        // Generate random direction
        float u = Random.value;
        float v = Random.value;
        float theta = u * 2.0f * math.PI;
        float phi = math.acos(2.0f * v - 1.0f);

        // Generate random distance within the sphere (cube root for uniform distribution)
        

        // Convert spherical coordinates to Cartesian coordinates
        float sinPhi = math.sin(phi);
        float x = center.x + r * sinPhi * math.cos(theta);
        float y = center.y + r * sinPhi * math.sin(theta);
        float z = center.z + r * math.cos(phi);

        return new float3(x, y, z);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
