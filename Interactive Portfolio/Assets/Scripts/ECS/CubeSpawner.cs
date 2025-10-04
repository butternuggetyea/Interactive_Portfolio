using Unity.Entities;
using Unity.Mathematics;

public struct CubeSpawner : IComponentData
{
    public Entity _cube;
    public static int _cubeCount;
    public float3 _spawnCenter;
    public float _distance;

   

    public static void SetCount(int count) 
    {
        _cubeCount = count;
    }

    public int Count() 
    {
        return _cubeCount;
    }

}
