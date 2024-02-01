using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ThrowData")]
public class ThrowData : StateData
{
    public float windupTime;
    public float timeBetweenThrows;
    public GameObject throwThing;
    public float throwAirSpeed;
}
