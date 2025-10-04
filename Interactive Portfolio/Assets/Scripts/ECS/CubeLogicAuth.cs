using Unity.Entities;
using UnityEngine;

class CubeLogicAuth : MonoBehaviour
{
    
}

class CubeLogicAuthBaker : Baker<CubeLogicAuth>
{
    public override void Bake(CubeLogicAuth authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new CubeLogic { });
    }
}
