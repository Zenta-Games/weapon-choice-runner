using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;
using Zenta.Core.Runtime.Managers;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<DynamicCube> attachedCubes;

    public List<CubePivot> cubePivots;
    
    public List<CubeRotater> cubeRotaters;

    public DynamicCube dynamicCubeSample;

    public TextMeshProUGUI countText;

    public int attachedCubeCount
    {
        get { return attachedCubes.Count; }
    }

    private void Start()
    {
        cubeRotaters = GetComponentsInChildren<CubeRotater>().ToList();

        cubePivots = new List<CubePivot>();

        attachedCubes = new List<DynamicCube>();

        GateManager.Instance.onTakeNewGate += OnTakeGate;

        for (int i = 0; i < cubeRotaters.Count; i++)
        {
            for (int j = 0; j < cubeRotaters[i].cubePivots.Count; j++)
            {
                cubePivots.Add(cubeRotaters[i].cubePivots[j]);
            }
        }

        SetTextToCount();
    }

    private CubePivot getNextPointTarget
    {
        get 
        {
            int emptyPivotCount = cubePivots.FindAll(x => x.attachedCube == null).Count;

            if (emptyPivotCount != 0)
            {
                return cubePivots.FindAll(x => x.attachedCube == false)[Random.Range(0, emptyPivotCount)];
            }
            else
            {
                return null;
            }
        }
    }

    public void OnNewCubeAttachedHero(DynamicCube dynamicCube) 
    {
        CubePivot newCubePivot = getNextPointTarget;

        if (newCubePivot == null)
        {
            dynamicCube.DestroyThis();
        }
        else
        {
            attachedCubes.Add(dynamicCube);

            newCubePivot.attachedCube = dynamicCube;

            dynamicCube.SetHero(getNextPointTarget);

            SetTextToCount();
        }

    }

    public void OnTakeGate(float value,MultiplerType multiplerType) 
    {
        int changingValue = 0;

        switch (multiplerType)
        {
            case MultiplerType.MULTIPLY:

                changingValue = (attachedCubeCount * (int)value) - attachedCubeCount;

                break;
         
            case MultiplerType.ADD:

                changingValue = (int)value;
                
                break;
            
            case MultiplerType.SUBTRACT:

                changingValue = (int)value * -1;

                break;
            
            case MultiplerType.DIVIDE:

                changingValue = (int)(((float)attachedCubeCount / value) - attachedCubeCount);

                break;
            default:
                break;
        }

        if (changingValue >= 0)
        {
            AddCubes(changingValue);
        }
        else
        {
            DestroyCubes(changingValue);
        }
    }

    private void DestroyCubes(int count) 
    {
        count = Mathf.Abs(count);

        for (int i = 0; i < count; i++)
        {
            attachedCubes[0].DestroyThis();

            attachedCubes.RemoveAt(0);
        }

    }

    public void DetachCube(DynamicCube dynamicCube) 
    {
        attachedCubes.Remove(dynamicCube);
        
        SetTextToCount();

        if (attachedCubes.Count == 0)
        {
            if (PlayerController.Instance.onFinishState == false)
            {
                GameManager.Instance.FailLevel();

                PlayerController.Instance.animator.Play("Death");

                countText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void AddCubes(int count) 
    {
        if (attachedCubeCount + count > 140)
        {
            count = 140 - attachedCubeCount;
        }

        for (int i = 0; i < count; i++)
        {
           DynamicCube dynamicCube = Instantiate(dynamicCubeSample,PlayerController.Instance.model.transform.position,Quaternion.identity,null).GetComponent<DynamicCube>();

           dynamicCube.AttachHero();
        }
    }

    public DynamicCube NewCube(Vector3 position)
    {
        return Instantiate(dynamicCubeSample, position, Quaternion.identity, null).GetComponent<DynamicCube>();
    }

    public void SetWeaponPose() 
    {
        transform.DOLocalMove(new Vector3(0,.5f,0),.1f);

        transform.DOScale(new Vector3(.4f,.4f,.4f),.1f);

        for (int i = 0; i < cubePivots.Count; i++)
        {
            cubePivots[i].SetWeaponScale();
        }
    }

    public void SetDefaultPose() 
    {
        transform.DOLocalMove(new Vector3(0,0,0), .1f);

        transform.DOScale(new Vector3(1f, 1f, 1f), .1f);
    }

    public void SetTextToCount() 
    {
        countText.text = attachedCubes.Count.ToString();
    }
}
