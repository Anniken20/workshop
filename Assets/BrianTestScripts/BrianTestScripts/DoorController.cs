using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{

    public bool isDoorOpen = false;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    float doorSpeed = 20f;
    private AudioSource audioSource;
    private bool playedOpen;

    public AudioClip doorOpen;

    //Sets door position to starting and highest opening point
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Awake ()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x, transform.position.y - 5f, transform.position.z);
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
            if (audioSource != null && doorOpen != null && playedOpen == false)
            {
                audioSource.PlayOneShot(doorOpen);
                playedOpen = true;
            }
        }
    }


    //Moves Door Down
    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
            playedOpen = false;
        }
    }
}
