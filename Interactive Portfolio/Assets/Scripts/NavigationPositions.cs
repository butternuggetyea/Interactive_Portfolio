using UnityEngine;

public class NavigationPositions : MonoBehaviour
{
    public Transform[] positions;

    public static Transform[] _positions;

    private void Awake()
    {
        _positions = positions;
    }


}
