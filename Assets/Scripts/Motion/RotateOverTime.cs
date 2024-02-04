using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    /*
     * Makes a 3D object rotate at some speed forever.
     * 
     * 7/27/23
     * 
     * Caden Henderson 
     * Updated 11/9/23
     */


    [SerializeField] private bool randomStart = true;
    [SerializeField] private bool dontRotate;

    [Tooltip("Roughly degrees per second; 360 means ~1 rps")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;

    private float randomOffset = 0f;

    private void Start()
    {
        if (randomStart) randomOffset = Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (dontRotate) return;
        transform.localEulerAngles += new Vector3(
            xAxis ? Time.deltaTime * rotationSpeed : 0f, 
            yAxis ? Time.deltaTime * rotationSpeed : 0f, 
            zAxis ? Time.deltaTime * rotationSpeed : 0f);
    }

    public void StopRotating()
    {
        dontRotate = true;
    }

    public void StartRotating()
    {
        dontRotate = false;
    }
}
