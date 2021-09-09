using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<DynamicCube> attachedCubes;

    public List<CubePivot> cubePivots;
    
    public List<CubeRotater> cubeRotaters;

    public int attachedCubeCount
    {
        get { return attachedCubes.Count; }
    }

    private void Start()
    {
        cubeRotaters = GetComponentsInChildren<CubeRotater>().ToList();

        cubePivots = new List<CubePivot>();

        attachedCubes = new List<DynamicCube>();

        for (int i = 0; i < cubeRotaters.Count; i++)
        {
            for (int j = 0; j < cubeRotaters[i].cubePivots.Count; j++)
            {
                cubePivots.Add(cubeRotaters[i].cubePivots[j]);
            }
        }
    }

    private CubePivot getNextPointTarget
    {
        get { return cubePivots.Find(x => x.hasAttach == false); }
    }

    public void OnNewCubeAttachedHero(DynamicCube dynamicCube) 
    {
        attachedCubes.Add(dynamicCube);

        CubePivot newCubePivot = getNextPointTarget;

        if (newCubePivot == null)
        {
            dynamicCube.DestroyThis();
        }
        else
        {
            getNextPointTarget.hasAttach = true;

            dynamicCube.SetHero(getNextPointTarget);
        }
    }

    private void Update()
    {
        
    }
}
