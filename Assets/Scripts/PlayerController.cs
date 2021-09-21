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

    private CompletedPanel completedPanel;

    public Transform model;

    private EnemyManager enemyManager;

    [MinMaxSlider(-5f, 5f)] public Vector2 horizontalLimits;

    private Finish finish;

    public float finishDistance;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 30;
    }

    private void Start()
    {
        enemyManager = EnemyManager.Instance;

        finish = Finish.Instance;

        finishDistance = finish.transform.position.z;
    }

    public void Initialize(Level level)
    {
        weapons = new List<IWeapon>();

        cubeManager = CubeManager.Instance;

        weapons = transform.GetComponentsInChildren<IWeapon>().ToList();

        playingPanel = PanelManager.Instance.GetPanel<PlayingPanel>();

        completedPanel = PanelManager.Instance.GetPanel<CompletedPanel>();

        playingPanel.onSelectWeapon += ActiveWeapon;

        playingPanel.touchField.onDrag += Move;

        playingPanel.ProgressBar.UpdateValue(transform.position.z / finishDistance);
    }

    private float movementSpeed = 10f;

    private float lerpedSpeed = 0f;

    private bool onFinishState = false;

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Playing && onFinishState == false)
        {
            lerpedSpeed = Mathf.Lerp(lerpedSpeed,movementSpeed,Time.deltaTime * 10f);

            if (enemyManager.HaveClosestEnemy(model.transform.position))
            {
                lerpedSpeed = Mathf.Lerp(lerpedSpeed,0, Time.deltaTime * 10f);
            }

            transform.Translate(Vector3.forward * Time.deltaTime * lerpedSpeed);

            playingPanel.ProgressBar.UpdateValue(transform.position.z / finishDistance);

            if (finishDistance - transform.position.z <= .5f)
            {
                onFinishState = true;

                StartFinishState();
            }
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

                weapon.WeaponState = WeaponState.ACTIVE;

                movementSpeed = 2f;

                for (int i = 0; i < weapon.RequiredCubeCount; i++)
                {
                    cubeManager.attachedCubes[0].cubePivot.attachedCube = null;

                    cubeManager.attachedCubes[0].SetWeapon(weapon.CubePivots[i]);

                    cubeManager.attachedCubes.RemoveAt(0);

                    yield return new WaitForEndOfFrame();

                    cubeManager.SetTextToCount();
                }

                yield return new WaitForSeconds(.1f);

                movementSpeed = 10f;

                weapon.Active();
            }
        }
    }

    public void StartFinishState() 
    {
        StartCoroutine(_StartFinishState());
    }

    private IEnumerator _StartFinishState() 
    {
        for (int i = 0; i < finish.requiredCubeCount; i++)
        {
            if (cubeManager.attachedCubeCount == 0)
            {

            }
            else
            {
                cubeManager.attachedCubes[0].cubePivot.attachedCube = null;

                finish.AtachCube(cubeManager.attachedCubes[0]);

                cubeManager.attachedCubes.RemoveAt(0);

                yield return new WaitForEndOfFrame();

                cubeManager.SetTextToCount();
            }
        }

        yield return new WaitForSeconds(.05f);

        while (cubeManager.attachedCubes.Count > 0)
        {
            cubeManager.attachedCubes[0].DestroyThis();

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.5f);

        GameManager.Instance.CompleteLevel();
    }
}
