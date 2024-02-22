using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Digs through a list of objectsToTurnGhostly and sets all of their shaders to the default ghost shader
 * 
 * Caden Henderson
 * 2/10/2024
 */

[System.Serializable]
public struct SMRenderer
{
    public Renderer _skinnedMeshRenderer;
    [HideInInspector] public Shader[] _shaders;
    [HideInInspector] public Color[] _colors;
}

public class PlayerPhaseMaterial : PhaseObject
{
    public SMRenderer[] objectsToTurnGhostly;
    public int materialDepth = 1;

    private void Start()
    {
        //cache shaders
        for (int i = 0; i < objectsToTurnGhostly.Length; ++i)
        {
            objectsToTurnGhostly[i]._shaders = new Shader[objectsToTurnGhostly[i]._skinnedMeshRenderer.materials.Length];
            objectsToTurnGhostly[i]._colors = new Color[objectsToTurnGhostly[i]._skinnedMeshRenderer.materials.Length];
            for (int j = 0; j < objectsToTurnGhostly[i]._skinnedMeshRenderer.materials.Length && j < materialDepth; ++j)
            {
                objectsToTurnGhostly[i]._shaders[j] = objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].shader;

                if(objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].HasColor("_Color"))
                    objectsToTurnGhostly[i]._colors[j] = objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].color;
            }
        }
    }
    protected override void OnEnter()
    {
        for (int i = 0; i < objectsToTurnGhostly.Length; ++i)
        {
            for (int j = 0; j < objectsToTurnGhostly[i]._shaders.Length && j < materialDepth; ++j)
            {
                objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].shader = GhostController.defaultGhostShader;

                if (objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].HasColor("_Color"))
                    objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].color = GhostController.defaultGhostColor;
            }
        }
    }

    protected override void OnExit()
    {
        for (int i = 0; i < objectsToTurnGhostly.Length; ++i)
        {
            for (int j = 0; j < objectsToTurnGhostly[i]._shaders.Length && j < materialDepth; ++j)
            {
                objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].shader = objectsToTurnGhostly[i]._shaders[j];

                if (objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].HasColor("_Color"))
                    objectsToTurnGhostly[i]._skinnedMeshRenderer.materials[j].color = objectsToTurnGhostly[i]._colors[j];
            }
        }
    }
}
