using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WE_Rock : MonoBehaviour , IWeapon
{
    public WeaponState weaponState;
    public WeaponState WeaponState { get { return weaponState; } set { weaponState = value; } }
    public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; } }

    public WeaponType weaponType;

    public Transform camReference;
    public Transform CamReference { get { return camReference; } }

    public Transform characterReference;
    public Transform CharacterReference { get { return camReference; } }

    public float actionTime;
    public float ActionTime { get { return actionTime; } }

    public bool CanActive()
    {
        return true;
    }

    public List<CubePivot> CubePivots { get { return cubePivots; } set { cubePivots = value; } }

    public List<CubePivot> cubePivots;

    public int RequiredCubeCount
    {
        get { return cubePivots.Count; }
    }

    private void Awake()
    {
        CubePivots = GetComponentsInChildren<CubePivot>().ToList();

        WeaponState = WeaponState.READY;
    }

    public void Active() { }

    public void Destroy() { }

    public CubePivot GetNextPivot()
    {
        return null;
    }
}
