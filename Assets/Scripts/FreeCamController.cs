using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamController : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float slowSpeed, normalSpeed, sprintSpeed;
    float currentSpeed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Movement();
            Rotation();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    void Rotation()
    {
        Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        transform.Rotate(mouseInput * sensitivity * Time.deltaTime * 50);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else if(Input.GetKey(KeyCode.LeftAlt))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        transform.Translate(input * currentSpeed * Time.deltaTime);
    }
}
