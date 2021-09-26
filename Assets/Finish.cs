using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public static Finish Instance;

    public List<CraftableObject> craftableObjects;

    public CraftableObject currentCraftableObject;

    [HideInInspector]
    public int plusPlacedCubeCount = 0;

    public int requiredCubeCount = 0;

    public TextMeshProUGUI cubeCountText;

    public Image countSlider;

    public Transform particles;

    public GameObject canvas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int placedCubeCount = PlayerPrefs.GetInt("placedCubeCount");

        int totalCraftableCubeCount = 0;

        for (int i = 0; i < craftableObjects.Count; i++)
        {
            totalCraftableCubeCount += craftableObjects[i].cubePivots.Count;

            if (placedCubeCount < totalCraftableCubeCount)
            {
                currentCraftableObject = craftableObjects[i];

                currentCraftableObject.gameObject.SetActive(true);

                plusPlacedCubeCount = placedCubeCount - (totalCraftableCubeCount - craftableObjects[i].cubePivots.Count);

                requiredCubeCount = craftableObjects[i].cubePivots.Count - plusPlacedCubeCount;

                currentCraftableObject.InitPlacement(plusPlacedCubeCount);

                UpdateUI();

                return;
            }
        }
    }

    public void AtachCube(DynamicCube dynamicCube) 
    {
        currentCraftableObject.AtachCube(dynamicCube);

        UpdateUI();

        canvas.SetActive(true);
    }

    public void UpdateUI() 
    {
        float rate = (float) currentCraftableObject.cubePivots.FindAll(x => x.attachedCube != null).Count / (float) currentCraftableObject.cubePivots.Count;

        countSlider.fillAmount = rate;

        if (rate == 1f)
        {
            particles.gameObject.SetActive(true);
        }

        cubeCountText.text = currentCraftableObject.name + " " + currentCraftableObject.cubePivots.FindAll(x=>x.attachedCube != null).Count+"/"+currentCraftableObject.cubePivots.Count;
    }
}
