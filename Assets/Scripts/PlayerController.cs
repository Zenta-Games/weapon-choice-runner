using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public List<IWeapon> weapons;

    private CubeManager cubeManager;

    public Transform weaponContent;

    private void Start()
    {
        weapons = new List<IWeapon>();

        cubeManager = CubeManager.Instance;

        weapons = weaponContent.transform.GetComponentsInChildren<IWeapon>().ToList();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 5f);
    }

    public void ActiveWeapon(WeaponType weaponType) 
    {
        IWeapon weapon = weapons.Find(x => x.WeaponType == weaponType);

        int currentCubeCount = cubeManager.attachedCubeCount;

        int requiredCubeCount = weapon.RequiredCubeCount;

        if (currentCubeCount >= requiredCubeCount)
        {
            if (weapon.CanActive())
            {
                weapon.Active();
            }
        }
    }
}
