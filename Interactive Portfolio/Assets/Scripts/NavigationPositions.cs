using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationPositions : MonoBehaviour
{
    public Transform[] positions;

    public static List<Transform> _positions;

    private void Awake()
    {
        _positions = positions.ToList();
    }


}
