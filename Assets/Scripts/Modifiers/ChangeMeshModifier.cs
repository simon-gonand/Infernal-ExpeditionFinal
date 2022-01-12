using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMeshModifier : IModifier
{
    [SerializeField]
    private Mesh newMesh;
    [SerializeField]
    private float newOutlineWidth;

    private float originalOutlineWidth;

    private List<Mesh> meshes = new List<Mesh>();

    private void Start()
    {
        originalOutlineWidth = PlayerManager.instance.players[0].outline.OutlineWidth;
    }

    protected override void StartBehaviour()
    {
        meshes.Clear();
        for (int i = 0; i < PlayerManager.instance.players.Count; ++i)
        {
            meshes.Add(PlayerManager.instance.players[i].selfRenderer.sharedMesh);
            PlayerManager.instance.players[i].selfRenderer.sharedMesh = newMesh;
            PlayerManager.instance.players[i].outline.OutlineWidth = newOutlineWidth;
        }
    }
    protected override void EndBehaviour()
    {
        for (int i = 0; i < PlayerManager.instance.players.Count; ++i)
        {
            PlayerManager.instance.players[i].selfRenderer.sharedMesh = meshes[i];
            PlayerManager.instance.players[i].outline.OutlineWidth = originalOutlineWidth;
        }
    }
}
