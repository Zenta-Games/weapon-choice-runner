using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Weapon_Selection : MonoBehaviour
{
    [HideInInspector]
    public bool isUsed;

    public WeaponType weaponType;

    public void Hide() 
    {
        isUsed = true;

        transform.GetChild(0).gameObject.SetActive(false);
    }
}
