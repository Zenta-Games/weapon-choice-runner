using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MultiplerHolder))]
[CanEditMultipleObjects]
public class MultiplerHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MultiplerHolder myScript = (MultiplerHolder)target;

        if (GUILayout.Button("Set Text"))
        {
            myScript.SetText();

            EditorUtility.SetDirty(myScript.gameObject);

            EditorUtility.SetDirty(myScript.functionText);

            EditorUtility.SetDirty(myScript.particlePositive.gameObject);

            EditorUtility.SetDirty(myScript.particleNegative.gameObject);

            myScript.functionText.SetAllDirty();
        }
    }
}
#endif

public class MultiplerHolder : MonoBehaviour
{
    public Action<float, MultiplerType> onTakeHolder; 

    public float factor;

    public MultiplerType multiplerType;

    public TextMeshProUGUI functionText;

    public Color positiveColor = new Color(77f / 255f, 0, 1), negativeColor = new Color(1, 62f / 255f, 0);

    public GameObject particlePositive, particleNegative;

    public List<GameObject> activeOnDestroyObjects;

    public void SetText() 
    {
        particlePositive.SetActive(false);

        particleNegative.SetActive(false);

        switch (multiplerType)
        {
            case MultiplerType.MULTIPLY:

                functionText.text = "×" + factor.ToString("F0");

                functionText.color = positiveColor;

                particlePositive.SetActive(true);

                break;
            case MultiplerType.ADD:
                functionText.text = "+" + factor.ToString("F0");

                functionText.color = positiveColor;

                particlePositive.SetActive(true);

                break;
            case MultiplerType.SUBTRACT:
                functionText.text = "-" + factor.ToString("F0");

                functionText.color = negativeColor;

                particleNegative.SetActive(true);

                break;
            case MultiplerType.DIVIDE:
                functionText.text = "÷" + factor.ToString("F0");

                functionText.color = negativeColor;

                particleNegative.SetActive(true);

                break;
            default:
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < activeOnDestroyObjects.Count; i++)
            {
                activeOnDestroyObjects[i].gameObject.SetActive(true);
            }

            onTakeHolder?.Invoke(factor, multiplerType);

            Destroy(this.gameObject);
        }
    }
}

public enum MultiplerType 
{
    MULTIPLY,
    ADD,
    SUBTRACT,
    DIVIDE
}
