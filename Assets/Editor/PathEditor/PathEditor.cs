using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    private SerializedProperty startWaypoint;
    private SerializedProperty waypointsList;
    private SerializedProperty linksList;
    private SerializedProperty loop;
    private SerializedProperty loopLink;

    private ReorderableList waypointsRList;
    private ReorderableList linksRList;

    private Waypoint selectedWaypoint;
    private int selectedWaypointIndex;
    private Link selectedLink;
    private int selectedLinkIndex;

    private Path pathScript;

    private NPCEventWindow npcEventWindow;

    private void OnEnable()
    {
        startWaypoint = serializedObject.FindProperty("startWaypoint");
        waypointsList = serializedObject.FindProperty("waypoints");
        linksList = serializedObject.FindProperty("links");
        loop = serializedObject.FindProperty("loop");
        loopLink = serializedObject.FindProperty("loopLink");

        pathScript = target as Path;

        waypointsRList = new ReorderableList(serializedObject, waypointsList);

        waypointsRList.drawHeaderCallback += HeaderWaypointCallback;
        waypointsRList.onSelectCallback += SelectWaypointCallback;
        waypointsRList.drawElementCallback += ElementWaypointCallback;
        waypointsRList.onAddCallback += AddWaypointCallback;
        waypointsRList.onRemoveCallback += RemoveWaypointCallback;
        waypointsRList.draggable = false;

        linksRList = new ReorderableList(serializedObject, linksList);
        linksRList.elementHeight = EditorGUIUtility.singleLineHeight * 3.0f;
        linksRList.drawHeaderCallback += HeaderLinkCallback;
        linksRList.onSelectCallback += SelectLinkCallback;
        linksRList.drawElementCallback += ElementLinkCallback;
        linksRList.draggable = false;
        linksRList.displayAdd = false;
        linksRList.displayRemove = false;
    }

    #region waypoints reorderable list
    private void HeaderWaypointCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Waypoints");
    }

    private void SelectWaypointCallback(ReorderableList rList)
    {
        SerializedProperty sp = waypointsList.GetArrayElementAtIndex(rList.index);
        Waypoint spWaypoint = sp.objectReferenceValue as Waypoint;
        if (selectedWaypoint == spWaypoint)
        {
            if (npcEventWindow == null || npcEventWindow.waypoint != spWaypoint)
            {
                npcEventWindow = (NPCEventWindow)EditorWindow.GetWindow(typeof(NPCEventWindow));
                npcEventWindow.Initialize(spWaypoint);
            }
        }
        else
        {
            selectedWaypoint = spWaypoint;
            selectedWaypointIndex = rList.index;
            selectedLink = null;
        }
    }

    private void ElementWaypointCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        rect.y += 2;
        Waypoint waypoint = (waypointsList.GetArrayElementAtIndex(index).objectReferenceValue as Waypoint);
        EditorGUI.BeginChangeCheck();
        Rect leftRect = new Rect(rect.x, rect.y, rect.width / 3, rect.height);
        Rect labelRect = new Rect(rect.x, rect.y, leftRect.width - leftRect.width / 4, rect.height);
        EditorGUI.LabelField(labelRect, "Waypoint " + index);

        Rect buttonRect = new Rect(labelRect.x + labelRect.width, rect.y, leftRect.width / 4, rect.height);
        if (GUI.Button(buttonRect, "+"))
        {
            GenericMenu addMenu = new GenericMenu();

            addMenu.AddItem(new GUIContent("Add waypoint above"), false, CreateWaypointAndLinks, index);
            addMenu.AddItem(new GUIContent("Add waypoint below"), false, CreateWaypointAndLinks, index + 1);

            addMenu.ShowAsContext();
        }

        Rect rightRect = new Rect(leftRect.x + leftRect.width, rect.y, rect.width - rect.width / 3, rect.height);
        Rect xRect = new Rect(rightRect.x, rect.y, rightRect.width / 2, rect.height);
        Rect labelxRect = new Rect(xRect.x, rect.y, xRect.width / 4, rect.height);
        Rect floatxRect = new Rect(xRect.x + labelxRect.width, rect.y, xRect.width - xRect.width / 4, rect.height);
        Rect yRect = new Rect(rightRect.x + xRect.width, rect.y, rightRect.width / 2, rect.height);
        Rect labelyRect = new Rect(yRect.x, rect.y, yRect.width / 4, rect.height);
        Rect floatyRect = new Rect(yRect.x + labelyRect.width, rect.y, yRect.width - yRect.width / 4, rect.height);

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = waypoint.transform.position;
        GUI.Label(labelxRect, "X");
        newPosition.x = EditorGUI.FloatField(floatxRect, GUIContent.none, waypoint.transform.position.x);
        GUI.Label(labelyRect, "Y");
        newPosition.z = EditorGUI.FloatField(floatyRect, GUIContent.none, waypoint.transform.position.z);
        waypoint.transform.position = newPosition;
        if (EditorGUI.EndChangeCheck())
        {
            if (linksList.arraySize > 0)
            {
                if (index < linksList.arraySize)
                {
                    Link link = linksList.GetArrayElementAtIndex(index).objectReferenceValue as Link;
                    if (link.pathPoints.Count == 0) return;
                    link.pathPoints[0] = (waypointsList.GetArrayElementAtIndex(index).objectReferenceValue as Waypoint).transform.position;
                }
                if (index > 0)
                {
                    Link link = linksList.GetArrayElementAtIndex(index - 1).objectReferenceValue as Link;
                    if (link.pathPoints.Count == 0) return;
                    link.pathPoints[link.pathPoints.Count - 1] = (waypointsList.GetArrayElementAtIndex(index).objectReferenceValue as Waypoint).transform.position;
                }
            }
        }
        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(waypoint.gameObject);
    }

    private void AddWaypointCallback(ReorderableList rlist)
    {
        CreateWaypoint(waypointsList.arraySize);
        if (waypointsList.arraySize > 1)
            CreateLinks();
        waypointsRList.index = waypointsList.arraySize - 1;
        waypointsRList.onSelectCallback(waypointsRList);
    }

    private void RemoveWaypointCallback(ReorderableList rlist)
    {
        selectedWaypoint = null;
        if (linksList.arraySize == 0)
        {
            waypointsList.DeleteArrayElementAtIndex(rlist.index);
            return;
        }
        if (rlist.index == 0)
        {
            linksList.DeleteArrayElementAtIndex(0);
            waypointsList.DeleteArrayElementAtIndex(rlist.index);
            return;
        }
        if (rlist.index == linksList.arraySize)
        {
            linksList.DeleteArrayElementAtIndex(linksList.arraySize - 1);
            waypointsList.DeleteArrayElementAtIndex(rlist.index);
            return;
        }

        linksList.DeleteArrayElementAtIndex(rlist.index - 1);
        linksList.DeleteArrayElementAtIndex(rlist.index - 1);
        Waypoint start = waypointsList.GetArrayElementAtIndex(rlist.index - 1).objectReferenceValue as Waypoint;
        Waypoint end = waypointsList.GetArrayElementAtIndex(rlist.index + 1).objectReferenceValue as Waypoint;
        CreateLink(start, end, rlist.index - 1);
        waypointsList.DeleteArrayElementAtIndex(rlist.index);
    }
    #endregion

    #region links reorderable list
    private void HeaderLinkCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Links");
    }

    private void SelectLinkCallback(ReorderableList rList)
    {
        SerializedProperty sp = linksList.GetArrayElementAtIndex(rList.index);
        selectedLink = sp.objectReferenceValue as Link;
        selectedLinkIndex = rList.index;
        selectedWaypoint = null;
    }
    private void ElementLinkCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        rect.y += 3;
        Rect leftRect = new Rect(rect.x, rect.y, rect.width / 5, rect.height);
        EditorGUI.LabelField(leftRect, index + " - " + (index + 1));

        Rect rightRect = new Rect(rect.x + leftRect.width, rect.y, rect.width - rect.width / 5, rect.height);
        Rect speedRect = new Rect(rect.x + rightRect.width / 2, rect.y, rightRect.width / 3, rect.height / 3);
        Rect camOffsetXRect = new Rect(rightRect.x, speedRect.y + speedRect.height, rightRect.width / 2, rect.height / 3);
        Rect camOffsetYRect = new Rect(camOffsetXRect.x + camOffsetXRect.width, speedRect.y + speedRect.height, rightRect.width / 2, rect.height / 3);
        Rect camZoomOffset = new Rect(rightRect.x, camOffsetXRect.y + camOffsetXRect.height, rightRect.width, rect.height / 3);
        Link currentLink = linksList.GetArrayElementAtIndex(index).objectReferenceValue as Link;
        EditorGUIUtility.labelWidth /= 4;
        EditorGUI.BeginChangeCheck();
        currentLink.speed = EditorGUI.FloatField(speedRect, "Speed", currentLink.speed);

        if (EditorGUI.EndChangeCheck())
        {
            if ((linksList.GetArrayElementAtIndex(index).objectReferenceValue as Link).speed < 0)
                (linksList.GetArrayElementAtIndex(index).objectReferenceValue as Link).speed = 0;
        }
        SerializedObject linkObject = new SerializedObject(currentLink);
        linkObject.Update();
        SerializedProperty camOffsetX = linkObject.FindProperty("XCameraOffset");
        SerializedProperty camOffsetY = linkObject.FindProperty("YCameraOffset");
        EditorGUIUtility.labelWidth *= 4;
        SerializedProperty offsetCamera = linkObject.FindProperty("UnzoomCameraOffset");
        EditorGUIUtility.labelWidth /= 2;
        EditorGUIUtility.fieldWidth /= 6;
        EditorGUI.PropertyField(camOffsetXRect, camOffsetX);
        EditorGUI.PropertyField(camOffsetYRect, camOffsetY);
        EditorGUI.PropertyField(camZoomOffset, offsetCamera);

        EditorGUIUtility.labelWidth *= 2;
        EditorGUIUtility.fieldWidth *= 6;

        linkObject.ApplyModifiedProperties();
    }
    #endregion

    private void CreateWaypointAndLinks(object index)
    {
        serializedObject.Update();
        int i = (int)index;
        CreateWaypoint(i);
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        if (i > 0 && i < waypointsList.arraySize - 1 && linksList.arraySize > 0)
        {
            linksList.DeleteArrayElementAtIndex(i - 1);
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        if (i > 0)
        {
            Waypoint start = waypointsList.GetArrayElementAtIndex(i - 1).objectReferenceValue as Waypoint;
            Waypoint end = waypointsList.GetArrayElementAtIndex(i).objectReferenceValue as Waypoint;
            if (linksList.arraySize == 0)
            {
                Link link = CreateLink(start, end, 0);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
            else
            {
                Link link = CreateLink(start, end, i - 1);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
        if (i < waypointsList.arraySize && waypointsList.arraySize > linksList.arraySize + 1)
        {
            Waypoint start = waypointsList.GetArrayElementAtIndex(i).objectReferenceValue as Waypoint;
            Waypoint end = waypointsList.GetArrayElementAtIndex(i + 1).objectReferenceValue as Waypoint;
            if (linksList.arraySize == 0)
            {
                Link link = CreateLink(start, end, 0);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
            else
            {
                Link link = CreateLink(start, end, i);
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void CreateWaypoint(int index)
    {
        int i = (int)index;
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.SetParent(pathScript.transform);
        obj.hideFlags = HideFlags.HideInHierarchy;
        Waypoint wp = obj.AddComponent<Waypoint>();


        waypointsList.InsertArrayElementAtIndex(i);
        waypointsList.GetArrayElementAtIndex(i).objectReferenceValue = wp;
        EditorUtility.SetDirty(obj);
        Undo.RegisterCreatedObjectUndo(obj, "Create waypoint");
    }

    private void CalculateLinks()
    {
        if (linksList.arraySize <= 0) return;

        for (int i = 0; i < linksList.arraySize; ++i)
        {
            Link link = linksList.GetArrayElementAtIndex(i).objectReferenceValue as Link;
            CalculateLink(link);
        }
    }

    private void CalculateLink(Link link)
    {
        NavMeshPath linkPath = new NavMeshPath();
        if (NavMesh.CalculatePath(link.start.transform.position, link.end.transform.position, 1 << NavMesh.GetAreaFromName("Navigable"), linkPath))
        {
            if (link.pathPoints.Count > 0)
                link.pathPoints.Clear();
            if (link.anchors.Count > 0)
                link.anchors.Clear();
        }
        for (int i = 0; i < linkPath.corners.Length; ++i)
        {
            link.pathPoints.Add(linkPath.corners[i]);
            if (linkPath.corners.Length < 2) continue;
            if (i == linkPath.corners.Length - 1) continue;
            Vector3 previousPoint;
            Vector3 nextPoint;
            if (i == 0 && i < linkPath.corners.Length - 1)
            {
                nextPoint = linkPath.corners[i + 1];
                Vector3 nextDist = nextPoint - linkPath.corners[i];
                previousPoint = linkPath.corners[i] - nextDist;
            }
            else
            {
                previousPoint = linkPath.corners[i - 1];
                nextPoint = linkPath.corners[i + 1];
            }

            Vector3 startTan = nextPoint - previousPoint;
            Vector3 startAnchor = linkPath.corners[i] + startTan * 0.1f;
            Vector3 dist = startAnchor - linkPath.corners[i];
            Vector3 endAnchor = nextPoint - dist;
            link.anchors.Add(startAnchor);
            link.anchors.Add(endAnchor);
        }
    }

    private Link CreateLink(Waypoint start, Waypoint end, int index)
    {
        GameObject go = new GameObject("Link");
        Link link = go.AddComponent<Link>();
        link.start = start;
        link.end = end;
        linksList.InsertArrayElementAtIndex(index);
        linksList.GetArrayElementAtIndex(index).objectReferenceValue = link;
        go.transform.SetParent(pathScript.transform);
        go.hideFlags = HideFlags.HideInHierarchy;
        return link;
    }

    private void CreateLinks()
    {
        for (int i = 1; i < waypointsList.arraySize; ++i)
        {
            Waypoint start = waypointsList.GetArrayElementAtIndex(i - 1).objectReferenceValue as Waypoint;
            Waypoint end = waypointsList.GetArrayElementAtIndex(i).objectReferenceValue as Waypoint;

            // Avoid to duplicate links
            bool linkAlreadyExist = false;
            for (int j = 0; j < linksList.arraySize; ++j)
            {
                Link l = linksList.GetArrayElementAtIndex(j).objectReferenceValue as Link;
                if (l.Equals(start, end))
                {
                    linkAlreadyExist = true;
                    break;
                }
            }
            if (linkAlreadyExist) continue;

            CreateLink(start, end, linksList.arraySize);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        startWaypoint.intValue = EditorGUILayout.IntSlider("Start Waypoint", startWaypoint.intValue, 0, waypointsList.arraySize - 1);
        waypointsRList.DoLayoutList();
        linksRList.DoLayoutList();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(loop);
        if (EditorGUI.EndChangeCheck())
        {
            if (loop.boolValue)
            {
                Waypoint start = waypointsList.GetArrayElementAtIndex(waypointsList.arraySize - 1).objectReferenceValue as Waypoint;
                Waypoint end = waypointsList.GetArrayElementAtIndex(0).objectReferenceValue as Waypoint;
                loopLink.objectReferenceValue = CreateLink(start, end, linksList.arraySize);
            }
            else
            {
                int index = pathScript.links.IndexOf(loopLink.objectReferenceValue as Link);
                linksList.DeleteArrayElementAtIndex(index);
            }
        }

        if (GUILayout.Button("Calculate Path"))
            CalculateLinks();
        if (GUILayout.Button("Calculate Selected Link"))
        {
            if (!selectedLink)
            {
                Debug.LogWarning("No link has been selected");
                return;
            }
            CalculateLink(selectedLink);
        }

        if (GUILayout.Button("Erase all"))
        {
            while (waypointsList.arraySize != 0 || linksList.arraySize != 0)
            {
                waypointsList.DeleteArrayElementAtIndex(0);
                if (linksList.arraySize == 0) continue;
                linksList.DeleteArrayElementAtIndex(0);
            }
            selectedLink = null;
            selectedWaypoint = null;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateAnchors(Vector3 prevPos, Vector3 newPos, int linkIndex, int anchorIndex)
    {
        Vector3 offset = newPos - prevPos;
        if (anchorIndex * 2 > pathScript.links[linkIndex].anchors.Count - 2)
        {
            if (linkIndex < pathScript.links.Count - 1)
            {
                ++linkIndex;
                pathScript.links[linkIndex].anchors[0] += offset;
                --linkIndex;
            }
            else if (loop.boolValue)
            {
                pathScript.links[0].anchors[0] += offset;
            }
        }
        else if (anchorIndex * 2 <= pathScript.links[linkIndex].anchors.Count - 2)
            pathScript.links[linkIndex].anchors[anchorIndex * 2] += offset;

        if (anchorIndex == 0)
        {
            if (linkIndex > 0)
            {
                --linkIndex;
                anchorIndex = pathScript.links[linkIndex].anchors.Count - 1;
                pathScript.links[linkIndex].anchors[anchorIndex] += offset;
            }
            else if (loop.boolValue)
            {
                pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1] += offset;
            }
        }
        else if (anchorIndex > 0)
        {
            pathScript.links[linkIndex].anchors[anchorIndex * 2 - 1] += offset;
        }
    }

    private Vector3 Create2DPositionHandles(Vector3 position, int linkIndex, int anchorIndex)
    {
        float constantZoom = HandleUtility.GetHandleSize(position);

        Vector3 prevPos = position;
        Handles.color = Handles.yAxisColor;
        EditorGUI.BeginChangeCheck();
        Vector3 newPos = Handles.Slider2D(position, Vector3.up,
            Vector3.right, Vector3.forward, 0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(0.1f, 0.1f));
        if (EditorGUI.EndChangeCheck())
        {
            if (linksList.arraySize > 0 && pathScript.links[linkIndex].anchors.Count > 0)
                UpdateAnchors(prevPos, newPos, linkIndex, anchorIndex);
        }
        position = newPos;

        Handles.color = Handles.xAxisColor;
        EditorGUI.BeginChangeCheck();
        Vector3 xPos = Handles.Slider(position, Vector3.right, 0.8f * constantZoom, Handles.ArrowHandleCap, Handles.SnapValue(0.1f, 0.1f));
        if (EditorGUI.EndChangeCheck())
        {
            if (linksList.arraySize > 0 && pathScript.links[linkIndex].anchors.Count > 0)
                UpdateAnchors(prevPos, xPos, linkIndex, anchorIndex);
        }
        position = xPos;

        Handles.color = Handles.zAxisColor;
        EditorGUI.BeginChangeCheck();
        Vector3 zPos = Handles.Slider(position, Vector3.forward, 0.8f * constantZoom, Handles.ArrowHandleCap, Handles.SnapValue(0.1f, 0.1f));
        if (EditorGUI.EndChangeCheck())
        {
            if (linksList.arraySize > 0 && pathScript.links[linkIndex].anchors.Count > 0)
                UpdateAnchors(prevPos, zPos, linkIndex, anchorIndex);
        }
        position = zPos;

        return position;
    }

    private int GetNbPoints()
    {
        int nbPoints = 0;
        for (int i = 0; i < linksList.arraySize; ++i)
        {
            Link currentLink = linksList.GetArrayElementAtIndex(i).objectReferenceValue as Link;
            nbPoints += currentLink.pathPoints.Count;
        }
        return nbPoints;
    }

    private void SceneBezier()
    {
        int nbPoints = GetNbPoints();
        if (nbPoints <= 0) return;
        for (int i = 0; i < linksList.arraySize; ++i)
        {
            Link currentLink = linksList.GetArrayElementAtIndex(i).objectReferenceValue as Link;
            for (int j = 0; j < currentLink.pathPoints.Count; ++j)
            {
                if (currentLink.pathPoints.Count < 2) break;
                if (j == currentLink.pathPoints.Count - 1) continue;
                Vector3 nextPoint = currentLink.pathPoints[j + 1];

                Handles.DrawBezier(currentLink.pathPoints[j], nextPoint, currentLink.anchors[j * 2], currentLink.anchors[j * 2 + 1], Color.green, null, 1.0f);
            }
        }
    }

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        for (int i = 0; i < waypointsList.arraySize; ++i)
        {
            Waypoint waypoint = waypointsList.GetArrayElementAtIndex(i).objectReferenceValue as Waypoint;
            float constantZoom = HandleUtility.GetHandleSize(waypoint.transform.position);
            Handles.DrawSolidDisc(waypoint.transform.position, Vector3.up, 0.2f * constantZoom);
        }
        Handles.color = Color.red;
        for (int i = 0; i < linksList.arraySize; ++i)
        {
            Link link = linksList.GetArrayElementAtIndex(i).objectReferenceValue as Link;
        }
        if (selectedWaypoint)
        {
            Undo.RecordObject(selectedWaypoint.transform, "Move Waypoints");
            int index = selectedWaypointIndex;
            int indexAnchor = 0;
            if (selectedWaypointIndex == linksList.arraySize && linksList.arraySize > 0)
            {
                --index;
                indexAnchor = pathScript.links[index].anchors.Count;
            }
            selectedWaypoint.transform.position = Create2DPositionHandles(selectedWaypoint.transform.position, index, indexAnchor / 2);
            EditorUtility.SetDirty(selectedWaypoint.transform);
            if (linksList.arraySize > 0)
            {
                if (selectedWaypointIndex < linksList.arraySize)
                {
                    Link link = linksList.GetArrayElementAtIndex(selectedWaypointIndex).objectReferenceValue as Link;
                    if (link.pathPoints.Count == 0) return;
                    link.pathPoints[0] = selectedWaypoint.transform.position;
                }
                if (selectedWaypointIndex > 0)
                {
                    Link link = linksList.GetArrayElementAtIndex(selectedWaypointIndex - 1).objectReferenceValue as Link;
                    if (link.pathPoints.Count == 0) return;
                    link.pathPoints[link.pathPoints.Count - 1] = selectedWaypoint.transform.position;
                }
                else if (loop.boolValue)
                {
                    Link link = linksList.GetArrayElementAtIndex(linksList.arraySize - 1).objectReferenceValue as Link;
                    if (link.pathPoints.Count == 0) return;
                    link.pathPoints[link.pathPoints.Count - 1] = selectedWaypoint.transform.position;
                }
            }
        }

        if (selectedLink && selectedLink.pathPoints.Count >= 2)
        {

            for (int i = 0; i < selectedLink.pathPoints.Count; ++i)
            {
                Undo.RecordObject(selectedLink, "Move Path points");
                selectedLink.pathPoints[i] = Create2DPositionHandles(selectedLink.pathPoints[i], selectedLinkIndex, i);
                if (linksList.arraySize > 0)
                {
                    if (selectedLinkIndex == 0 && i == 0 && loop.boolValue)
                    {
                        Link link = linksList.GetArrayElementAtIndex(linksList.arraySize - 1).objectReferenceValue as Link;
                        if (link.pathPoints.Count == 0) return;
                        link.pathPoints[link.pathPoints.Count - 1] = selectedLink.pathPoints[i];
                    }
                    if (selectedLinkIndex == linksList.arraySize - 1 && i == selectedLink.pathPoints.Count - 1 && loop.boolValue)
                    {
                        Link link = linksList.GetArrayElementAtIndex(0).objectReferenceValue as Link;
                        if (link.pathPoints.Count == 0) return;
                        link.pathPoints[0] = selectedLink.pathPoints[i];
                    }
                }

                if (selectedLink.pathPoints.Count < 2) continue;
                if (i == selectedLink.pathPoints.Count - 1) continue;
                float constantZoom = HandleUtility.GetHandleSize(selectedLink.anchors[i * 2]);

                if (i == 0)
                {
                    if (selectedLinkIndex > 0)
                    {
                        EditorGUI.BeginChangeCheck();
                        pathScript.links[selectedLinkIndex - 1].anchors[pathScript.links[selectedLinkIndex - 1].anchors.Count - 1] =
                            Handles.Slider2D(pathScript.links[selectedLinkIndex - 1].anchors[pathScript.links[selectedLinkIndex - 1].anchors.Count - 1],
                            Vector3.up, Vector3.right, Vector3.forward, 0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                        if (EditorGUI.EndChangeCheck())
                        {
                            Vector3 dist = pathScript.links[selectedLinkIndex - 1].anchors[pathScript.links[selectedLinkIndex - 1].anchors.Count - 1] - selectedLink.pathPoints[i];
                            selectedLink.anchors[i * 2] = selectedLink.pathPoints[i] - dist;
                        }
                        Handles.DrawLine(pathScript.links[selectedLinkIndex - 1].anchors[pathScript.links[selectedLinkIndex - 1].anchors.Count - 1], selectedLink.pathPoints[i]);
                    }
                    else if (pathScript.loop)
                    {
                        EditorGUI.BeginChangeCheck();
                        pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1] =
                            Handles.Slider2D(pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1],
                            Vector3.up, Vector3.right, Vector3.forward, 0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                        if (EditorGUI.EndChangeCheck())
                        {
                            Vector3 dist = pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1] - selectedLink.pathPoints[i];
                            selectedLink.anchors[i * 2] = selectedLink.pathPoints[i] - dist;
                        }
                        Handles.DrawLine(pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1], selectedLink.pathPoints[i]);
                    }
                }
                EditorGUI.BeginChangeCheck();
                selectedLink.anchors[(i * 2)] = Handles.Slider2D(selectedLink.anchors[i * 2], Vector3.up, Vector3.right, Vector3.forward,
                    0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                if (EditorGUI.EndChangeCheck())
                {
                    if (i > 0)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2] - selectedLink.pathPoints[i];
                        selectedLink.anchors[i * 2 - 1] = selectedLink.pathPoints[i] - dist;
                    }
                    else if (selectedLinkIndex > 0)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2] - selectedLink.pathPoints[i];
                        pathScript.links[selectedLinkIndex - 1].anchors[pathScript.links[selectedLinkIndex - 1].anchors.Count - 1] = selectedLink.pathPoints[i] - dist;
                    }
                    else if (selectedLinkIndex == 0 && pathScript.loop)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2] - selectedLink.pathPoints[i];
                        pathScript.links[pathScript.links.Count - 1].anchors[pathScript.links[pathScript.links.Count - 1].anchors.Count - 1] = selectedLink.pathPoints[i] - dist;
                    }
                }


                constantZoom = HandleUtility.GetHandleSize(selectedLink.anchors[i * 2 + 1]);
                if (i * 2 + 1 > selectedLink.anchors.Count - 2 && selectedLinkIndex < pathScript.links.Count - 1)
                {
                    EditorGUI.BeginChangeCheck();
                    pathScript.links[selectedLinkIndex + 1].anchors[0] =
                        Handles.Slider2D(pathScript.links[selectedLinkIndex + 1].anchors[0],
                        Vector3.up, Vector3.right, Vector3.forward, 0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 dist = pathScript.links[selectedLinkIndex + 1].anchors[0] - selectedLink.pathPoints[i + 1];
                        selectedLink.anchors[i * 2 + 1] = selectedLink.pathPoints[i + 1] - dist;
                    }
                    Handles.DrawLine(pathScript.links[selectedLinkIndex + 1].anchors[0], selectedLink.pathPoints[i + 1]);
                }
                else if (i * 2 + 1 > selectedLink.anchors.Count - 2 && pathScript.loop)
                {
                    EditorGUI.BeginChangeCheck();
                    pathScript.links[0].anchors[0] =
                        Handles.Slider2D(pathScript.links[0].anchors[0],
                        Vector3.up, Vector3.right, Vector3.forward, 0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 dist = pathScript.links[0].anchors[0] - selectedLink.pathPoints[i + 1];
                        selectedLink.anchors[i * 2 + 1] = selectedLink.pathPoints[i + 1] - dist;
                    }
                    Handles.DrawLine(pathScript.links[0].anchors[0], selectedLink.pathPoints[i + 1]);
                }

                EditorGUI.BeginChangeCheck();
                selectedLink.anchors[i * 2 + 1] = Handles.Slider2D(selectedLink.anchors[i * 2 + 1], Vector3.up, Vector3.right, Vector3.forward,
                    0.1f * constantZoom, Handles.DotHandleCap, Handles.SnapValue(1.0f, 1.0f));
                if (EditorGUI.EndChangeCheck())
                {
                    if (i != selectedLink.pathPoints.Count - 2)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2 + 1] - selectedLink.pathPoints[i + 1];
                        selectedLink.anchors[i * 2 + 2] = selectedLink.pathPoints[i + 1] - dist;
                    }
                    else if (selectedLinkIndex < pathScript.links.Count - 1)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2 + 1] - selectedLink.pathPoints[i + 1];
                        pathScript.links[selectedLinkIndex + 1].anchors[0] = selectedLink.pathPoints[i + 1] - dist;
                    }
                    else if (selectedLinkIndex == pathScript.links.Count - 1 && pathScript.loop)
                    {
                        Vector3 dist = selectedLink.anchors[i * 2 + 1] - selectedLink.pathPoints[i + 1];
                        pathScript.links[0].anchors[0] = selectedLink.pathPoints[i + 1] - dist;
                    }
                }
                Vector3 nextPoint = selectedLink.pathPoints[i + 1];
                Handles.DrawLine(selectedLink.pathPoints[i], selectedLink.anchors[i * 2]);
                Handles.DrawLine(nextPoint, selectedLink.anchors[i * 2 + 1]);
                EditorUtility.SetDirty(selectedLink);
            }

            selectedLink.start.transform.position = selectedLink.pathPoints[0];
            if (selectedLinkIndex > 0)
            {
                Link previousLink = linksList.GetArrayElementAtIndex(selectedLinkIndex - 1).objectReferenceValue as Link;
                previousLink.pathPoints[previousLink.pathPoints.Count - 1] = selectedLink.pathPoints[0];
                previousLink.end.transform.position = selectedLink.pathPoints[0];
            }

            selectedLink.end.transform.position = selectedLink.pathPoints[selectedLink.pathPoints.Count - 1];
            if (selectedLinkIndex < linksList.arraySize - 1)
            {
                Link nextLink = linksList.GetArrayElementAtIndex(selectedLinkIndex + 1).objectReferenceValue as Link;
                nextLink.pathPoints[0] = selectedLink.end.transform.position;
                nextLink.start.transform.position = nextLink.pathPoints[0];
            }
        }
        SceneBezier();
    }
}
