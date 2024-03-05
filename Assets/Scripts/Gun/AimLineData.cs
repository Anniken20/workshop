using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GunData/AimLineData")]
public class AimLineData : ScriptableObject
{
    public Material scrollLineMaterial;
    public string key_scrollSpeed;
    public string key_albedo;
    public Color normalAimColor;
    public Color shootableAimColor;
    public Color lassoableAimColor;
    public float normalScrollSpeed;
    public float shootableScrollSpeed;
    public float lassoableScrollSpeed;

}
