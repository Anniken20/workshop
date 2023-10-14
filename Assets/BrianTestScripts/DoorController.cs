using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{

    public bool isDoorOpen = false;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    float doorSpeed = 10f;

    //Sets door position to starting and highest opening point
    void Awake ()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (isDoorOpen)
        { 
            OpenDoor();
        }
        else if (!isDoorOpen)
        {
            CloseDoor();
        }
    }

    //Moves Door Up
    void OpenDoor()
    {
        if (transform.position != doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        }
    }


    //Moves Door Down
    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }
}
