using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCube : MonoBehaviour
{
    [HideInInspector]
    public CubePivot cubePivot;

    public CubeState cubeState;
    
    public CubeState CubeState 
    {
        get { return cubeState; }

        set
        {
            cubeState = value;
         
            OnCubeStateChanged(cubeState);
        }
    }

    public void OnCubeStateChanged(CubeState cubeState) 
    {
        switch (cubeState)
        {
            case CubeState.ON_HERO:
                Debug.Log("gggg");

                CubeManager.Instance.OnNewCubeAttachedHero(this);
                break;

            case CubeState.ON_WEAPON:

                break;
         
            case CubeState.COLLECTABLE:

                break;
            
            case CubeState.DESTROYED:

                break;
            
            default:
                break;
        }
    }

    [Button("Attach Hero")]
    public void AttachHero() 
    {
        CubeState = CubeState.ON_HERO;
    }

    [Button("Attach Weapon")]
    public void AttachWeapon()
    {
        CubeState = CubeState.ON_WEAPON;
    }

    [Button("Destroy This")]
    public void DestroyThis()
    {
        CubeState = CubeState.DESTROYED;
    }

    [Button("Set Collectable Mode")]
    public void SetCollectableMode()
    {
        CubeState = CubeState.COLLECTABLE;
    }

    public void SetHero(CubePivot pivotTarget) 
    {
        cubePivot = pivotTarget;
    }

    public void SetWeapon(CubePivot cubePivot) 
    {
        
    }

    private void Update()
    {
        if (cubePivot != null)
        {
            transform.position = Vector3.Slerp(transform.position, cubePivot.cubePoint.transform.position, Time.deltaTime * 5f);
        }
    }
}

public enum CubeState 
{
    ON_HERO,
    ON_WEAPON,
    COLLECTABLE,
    DESTROYED
}
