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

        serializedObject.Update();

        while (treasuresInScene.arraySize > 0)
        {
            treasuresInScene.DeleteArrayElementAtIndex(0);
        }

        // Add all treasures in the scene in list
        Treasure[] treasures = GameObject.FindObjectsOfType<Treasure>();
        foreach (Treasure treasure in treasures)
        {
            treasuresInScene.InsertArrayElementAtIndex(treasuresInScene.arraySize);
            treasuresInScene.GetArrayElementAtIndex(treasuresInScene.arraySize - 1).objectReferenceValue = treasure;
        }

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
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
