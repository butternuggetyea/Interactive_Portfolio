using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class CubeSpawnerAuth : MonoBehaviour
{
    public GameObject _cube;
    public int _count;
    public float3 _spawnCenter;
    public float _distance;

}

class CubeSpawnerAuthBaker : Baker<CubeSpawnerAuth>
{
    public override void Bake(CubeSpawnerAuth authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new CubeSpawner 
        {
            _cube = GetEntity(authoring._cube,TransformUsageFlags.Dynamic),
            _distance = authoring._distance,
            _spawnCenter = authoring._spawnCenter,
            
        });
    }
}
