using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData/DaughterData")]
public class DaughterData : StateData
{
    //public GameObject[] graves;
    public float shakeSpeed;
    public float shakeDuration;
    public float shakeAmount;
    public float popDelay;
    //public GraveShaker graveShaker;
    public GraveContainer graveContainer;

}
