using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WE_Arrow : MonoBehaviour
{
    public List<CubePivot> cubePivots;

    private void Start()
    {
        cubePivots = GetComponentsInChildren<CubePivot>().ToList();
    }
}
