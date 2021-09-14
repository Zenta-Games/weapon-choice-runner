using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenta.Core.Runtime.Managers;
using Zenta.Core.Runtime.UI.Panel.Panels;
using Zenta.Core.Runtime;
using Zenta.Core.Runtime.Interfaces;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour , IInitializable
{
    public static PlayerController Instance;

    public List<IWeapon> weapons;

    private CubeManager cubeManager;

    private PlayingPanel playingPanel;

    public Transform model;

    [MinMaxSlider(-5f, 5f)] public Vector2 horizontalLimits;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 30;
    }

    public void Initialize(Level level)
    {
        weapons = new List<IWeapon>();

        cubeManager = CubeManager.Instance;

        weapons = transform.GetComponentsInChildren<IWeapon>().ToList();

        playingPanel = PanelManager.Instance.GetPanel<PlayingPanel>();

        playingPanel.onSelectWeapon += ActiveWeapon;

        playingPanel.touchField.onDrag += Move;
    }


    private float movementSpeed = 8f;

    private float lerpedSpeed = 0f;

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Playing)
        {

            lerpedSpeed = Mathf.Lerp(lerpedSpeed,movementSpeed,Time.deltaTime * 10f);

            transform.Translate(Vector3.forward * Time.deltaTime * lerpedSpeed);
        }
    }

    private void Move(Vector2 movementVector) 
    {
        float xValue = movementVector.x / 30f + model.transform.localPosition.x;

        xValue = Mathf.Clamp(xValue, horizontalLimits.x, horizontalLimits.y);

        Vector3 fixedLocalPosition = new Vector3(xValue,0, 0);

        model.transform.localPosition = Vector3.Lerp(model.transform.localPosition,fixedLocalPosition,Time.deltaTime * 20f); 
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
                CameraController.Instance.SetTransformReference(weapon.CamReference,weapon.ActionTime);

                //cubeManager.SetWeaponPose();

                movementSpeed = 2f;

                for (int i = 0; i < weapon.RequiredCubeCount; i++)
                {
                    cubeManager.attachedCubes[0].cubePivot.attachedCube = null;

                    cubeManager.attachedCubes[0].cubePivot.hasAttach = false;

                    cubeManager.attachedCubes[0].SetWeapon(weapon.CubePivots[i]);

                    cubeManager.attachedCubes.RemoveAt(0);

                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(.1f);

                movementSpeed = 8f;

                weapon.Active();
            }
        }
    }

  
}
