using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WeaponSelectionButton : MonoBehaviour
{
    public WeaponType weaponType;

    public Action<WeaponType> onSelectWeapon;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => { onSelectWeapon?.Invoke(weaponType); });
    }
}
