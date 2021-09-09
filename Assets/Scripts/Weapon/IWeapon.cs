using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IWeapon
{
    WeaponType WeaponType { get; set; }

    List<CubePivot> CubePivots { get; set; }

    bool CanActive();

    int RequiredCubeCount { get; }

    CubePivot GetNextPivot();

    void Active();

    void Destroy();
}

public enum WeaponType 
{
    ROCK,
    ARROW,
    BLADE,
    UMBRELLA,
    BRIDGE
}
