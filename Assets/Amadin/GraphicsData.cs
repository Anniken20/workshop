using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Graphics Data", menuName = "ScriptableObjects/GraphicsData")]
public class GraphicsData : ScriptableObject
{
    // Resolution
    public int resolutionWidth;
    public int resolutionHeight;

    // Fullscreen
    public bool fullscreen;

    // Quality of the game
    public int qualityLevel;
}
