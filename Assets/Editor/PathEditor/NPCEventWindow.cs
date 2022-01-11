using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class NPCEventWindow : EditorWindow
{
    public Waypoint waypoint;
    private SerializedObject waypointSO;

    public void Initialize(Waypoint w)
    {
        waypoint = w;
        waypointSO = new SerializedObject(waypoint);
    }

    private void OnGUI()
    {
        waypointSO.Update();
        EditorGUILayout.PropertyField(waypointSO.FindProperty("ev"), true);
        waypointSO.ApplyModifiedProperties();
    }
}