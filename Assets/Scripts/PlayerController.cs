using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenta.Core.Runtime.Managers;
using Zenta.Core.Runtime.UI.Panel.Panels;
using Zenta.Core.Runtime;
using Zenta.Core.Runtime.Interfaces;

public class PlayerController : MonoBehaviour , IInitializable
{
    public List<IWeapon> weapons;

    private CubeManager cubeManager;

    public Transform weaponContent;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public void Initialize(Level level)
    {
        weapons = new List<IWeapon>();

        cubeManager = CubeManager.Instance;

        weapons = weaponContent.transform.GetComponentsInChildren<IWeapon>().ToList();

        PanelManager.Instance.GetPanel<PlayingPanel>().onSelectWeapon += ActiveWeapon;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Playing)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 5f);
        }
    }

    public void ActiveWeapon(WeaponType weaponType) 
    {
        StartCoroutine(_ActiveWeapon(weaponType));
    }

    private IEnumerator _ActiveWeapon(WeaponType weaponType) 
    {
        IWeapon weapon = weapons.Find(x => x.WeaponType == weaponType);

        int currentCubeCount = cubeManager.attachedCubeCount;

        int requiredCubeCount = weapon.RequiredCubeCount;

        if (currentCubeCount >= requiredCubeCount)
        {
            if (weapon.CanActive())
            {
                for (int i = 0; i < weapon.RequiredCubeCount; i++)
                {
                    cubeManager.attachedCubes[0].cubePivot.hasAttach = false;

                    cubeManager.attachedCubes[0].SetWeapon(weapon.CubePivots[i]);

                    cubeManager.attachedCubes.RemoveAt(0);

                    yield return new WaitForEndOfFrame();
                    //;
                }

                for (int i = 0; i < weapon.RequiredCubeCount; i++)
                {

                }
                //weapon.Active();
            }
        }
    }

  
}
