using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.ProBuilder.Shapes;

partial struct CubeLogicSystem : ISystem
{
 
    public void OnCreate(ref SystemState state)
    {
        
    }


    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach ((RefRW<CubeLogic> cube, RefRW<LocalTransform> transform) in SystemAPI.Query<RefRW<CubeLogic>, RefRW<LocalTransform>>())
        {
            // Get current position relative to center
            float3 relativePos = transform.ValueRO.Position - cube.ValueRO._center;

            // If we're at center, initialize position
            if (math.lengthsq(relativePos) < 0.01f)
            {
                relativePos = new float3(cube.ValueRO._distance, 0, 0);
                transform.ValueRW.Position = cube.ValueRO._center + relativePos;
            }

            // Rotate around Y-axis (vertical axis)
            float rotationSpeed = 1.0f; // radians per second
            float rotationAngle = rotationSpeed * deltaTime;

            quaternion rotation = quaternion.RotateY(rotationAngle);
            float3 rotatedPos = math.mul(rotation, relativePos);

            // Update position
            transform.ValueRW.Position = cube.ValueRO._center + rotatedPos;

            // Optional: Rotate the cube to face the direction of movement
            if (math.lengthsq(rotatedPos) > 0.01f)
            {
                float3 moveDirection = math.normalize(rotatedPos - relativePos);
                transform.ValueRW.Rotation = quaternion.LookRotation(moveDirection, math.up());
            }
        }
    }


    public void OnDestroy(ref SystemState state)
    {
        
    }
}
