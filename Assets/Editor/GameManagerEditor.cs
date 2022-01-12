using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    SerializedProperty modifiers;

    private void OnEnable()
    {
        modifiers = serializedObject.FindProperty("modifiers");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (GUILayout.Button("Reset Progress"))
        {
            SaveData.instance.ResetSave();
            SerializationManager.Save(SaveData.instance);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
