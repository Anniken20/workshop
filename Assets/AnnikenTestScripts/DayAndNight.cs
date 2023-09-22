using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    Vector3 rot=Vector3.zero;

    float degpersec=6;

    // Update is called once per frame
    void Update()
    {
        rot.x=degpersec*Time.deltaTime;
        transform.Rotate(rot,Space.World);
    }
}

//This script just rotates the X axis
//directional light only changes
// rotation per sec can be changed + add the lamp function
// Change the directional light location to change the funky shadows
