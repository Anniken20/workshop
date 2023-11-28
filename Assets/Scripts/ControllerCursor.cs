using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCursor : MonoBehaviour
{
    public GameObject cursor;
    public float sensitivity;

    private void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            cursor.transform.position += new Vector3(-Time.deltaTime * sensitivity, 0);
        }
        if (Input.GetKey(KeyCode.L))
        {
            cursor.transform.position += new Vector3(Time.deltaTime * sensitivity, 0);
        }
        if (Input.GetKey(KeyCode.I))
        {
            cursor.transform.position += new Vector3(0, Time.deltaTime * sensitivity);
        }
        if (Input.GetKey(KeyCode.K))
        {
            cursor.transform.position += new Vector3(0, -Time.deltaTime * sensitivity);
        }
    }
}
