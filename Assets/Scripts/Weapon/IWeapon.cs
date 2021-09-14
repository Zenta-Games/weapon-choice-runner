using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IWeapon
{
    WeaponType WeaponType { get; set; }

    List<CubePivot> CubePivots { get; set; }

    WeaponState WeaponState { get; set; }

    bool CanActive();

    int RequiredCubeCount { get; }

    Transform CamReference { get;}

    Transform CharacterReference { get; }

    float ActionTime { get; }

    CubePivot GetNextPivot();

    void Active();

    void Destroy();
}

public enum WeaponState 
{
    READY,
    ACTIVE
}

public enum WeaponType 
{
    ROCK,
    ARROW,
    BLADE,
    UMBRELLA,
    BRIDGE
}
