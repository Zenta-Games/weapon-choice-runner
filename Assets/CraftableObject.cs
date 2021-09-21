using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftableObject : MonoBehaviour
{
    public List<CubePivot> cubePivots;

    [Button("SetCubePivots")]
    public void SetCubePivots() 
    {
        cubePivots = GetComponentsInChildren<CubePivot>().ToList();
    }

    public void InitPlacement(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DynamicCube newCube = CubeManager.Instance.NewCube(cubePivots[i].transform.position);

            newCube.AtachFinishObject();
        }
    }

    public void AtachCube(DynamicCube dynamicCube)
    {
        CubePivot nextPivot = getNextPointTarget;

        nextPivot.attachedCube = dynamicCube;

        dynamicCube.cubePivot = nextPivot;
    }

    private CubePivot getNextPointTarget
    {
        get
        {
            int emptyPivotCount = cubePivots.FindAll(x => x.attachedCube == null).Count;

            if (emptyPivotCount != 0)
            {
                return cubePivots.Find(x => x.attachedCube == false);
            }
            else
            {
                return null;
            }
        }
    }
}
