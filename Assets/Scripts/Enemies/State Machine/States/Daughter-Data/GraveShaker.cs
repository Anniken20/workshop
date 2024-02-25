using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveShaker : MonoBehaviour
{
    public bool status;
    public float shakeSpeed;
    public float shakeAmount;
    private Vector3 ogPos;
    public string axis;
    
    void Start()
    {
        ogPos = this.transform.position;
    }
    void FixedUpdate()
    {
        if(status)
        {
           if (axis == "X")
            {
                this.transform.position = new Vector3(ogPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, ogPos.y, ogPos.z);
            }
            if(axis == "Z")
            {
                this.transform.position = new Vector3(ogPos.x, ogPos.y, ogPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            }
        }
        else
        {
            this.transform.position = ogPos;
        }
    }
    public void SetValues(bool svStatus, float svshakeSpeed, float svshakeAmount, string svAxis)
    {
        status =svStatus;
        shakeSpeed = svshakeSpeed;
        shakeAmount = svshakeAmount;
        axis = svAxis;
    }
}
