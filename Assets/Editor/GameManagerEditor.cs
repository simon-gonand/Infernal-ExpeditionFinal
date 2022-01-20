using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    SerializedProperty treasuresInScene;

    private void OnEnable()
    {
        treasuresInScene = serializedObject.FindProperty("treasuresInScene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        if (GUILayout.Button("Reset Progress"))
        {
            SaveData.instance.ResetSave();
            SerializationManager.Save(SaveData.instance);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
