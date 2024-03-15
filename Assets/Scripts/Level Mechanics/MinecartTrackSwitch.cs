using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MinecartTrackSwitch : MinecartWall
{
    public SplineContainer newSpline;
    public MinecartMechanic minecart;
    public override void OnHit()
    {
        minecart.SwitchTracks(newSpline);
    }
}
