/*
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class MyClass : MonoBehaviour
{
    [Header("Text Attributes")]
    [TextArea]
    [Tooltip("A string using the TextArea attribute")]
    [SerializeField]
    private string descriptionTextArea;

    [Multiline]
    [Tooltip("A string using the MultiLine attribute")]
    [SerializeField]
    private string descriptionMultiLine;

    [Header("Numeric Attributes")]
    [Tooltip("A float using the Range attribute")]
    [Range(-5f, 5f)]
    
    [SerializeField]
    private float rangedFloat;

    [Space]
    [Tooltip("An integer using the Range attribute")]
    [Range(-5, 5)]
    [SerializeField]
    private int rangedInt;

    [Header("Color Attributes")]
    [Tooltip("A color using the ColorUsage attribute")]

    [SerializeField]
    private Color colorNormal;

    [ColorUsage(false)]
    [SerializeField]
    private Color colorNoAlpha;

    [ColorUsage(true, true)]
    [SerializeField]
    private Color colorHdr;

    [ContextMenu("Choose Random Values")]
    private void ChooseRandomValues()
    {
        rangedFloat = Random.Range(-5f, 5f);
        rangedInt = Random.Range(-5, 5);
    }

    [Header("Context Menu Items")]
    [ContextMenuItem("RandomValue", "RandomizeValueFromRightClick")]
    [Tooltip("A float that can be randomized from the context menu")]
    [SerializeField]
    private float randomValue;

    [Header("Read-Only Attributes")]
    [Tooltip("A read-only float attribute")]
    [ReadOnly]
    [SerializeField]
    private float readOnlyFloat;

    private void RandomizeValueFromRightClick()
    {
        randomValue = Random.Range(-5f, 5f);
    }

    //[Header("Other Attributes")]
    //[Tooltip("A field that is hidden in the inspector")]
    //[HideInInspector]
    //[SerializeField]
    //private int hiddenField = 0; // This field is hidden in the inspector

    //[Tooltip("A field that is serialized but not visible in the inspector")]
    //[SerializeField]
    //private int serializedField = 1; // This field is serialized but not visible in the inspector 

    [ContextMenu("Reset Values")]
    private void ResetValues()
    {
        descriptionTextArea = string.Empty;
        descriptionMultiLine = string.Empty;
        rangedFloat = 0f;
        rangedInt = 0;
        colorNormal = Color.white;
        //hiddenField = 0; 
        //serializedField = 0;
    }
}

public class ReadOnlyAttributeTest : PropertyAttribute // Custom attribute to make a field read-only in the inspector
{
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawerTest : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) // Override the OnGUI method to make the property read-only
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label); // Draw the property field as disabled
        GUI.enabled = true;
    }
}
*/