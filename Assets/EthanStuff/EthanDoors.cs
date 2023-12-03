using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanDoors : MonoBehaviour
{
    [SerializeField] DoorController doorController;
    [SerializeField] InventoryManager.AllItems requiredItem;


    [SerializeField] bool isDoorOpenSwitch;
    [SerializeField] bool isDoorCloseSwitch;

    float switchSizeY;
    Vector3 switchUpPos;
    Vector3 switchDownPos;
    float switchSpeed = 1f;
    float switchDelay = 0.2f;
    bool isPressingSwitch = false;

    //Ethan Testing
    [Header("Door Types")]
    [SerializeField] bool KeyDoor;
    [SerializeField] bool BreakObjDoor;
    [SerializeField] bool PressurePlateDoor;
    [SerializeField] GameObject DoorOpenerObj;


    void Awake()
    {
        switchSizeY = transform.localScale.y / 2;

        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressingSwitch && !PressurePlateDoor)
        {
            MoveSwitchDown();
        }
        else if (!isPressingSwitch)
        {
            MoveSwitchUp();
        }

        if (BreakObjDoor && DoorOpenerObj == null)
        {
            if (isDoorOpenSwitch && !doorController.isDoorOpen)
            {
                doorController.isDoorOpen = !doorController.isDoorOpen;
            }
            else if (isDoorCloseSwitch && doorController.isDoorOpen)
            {
                doorController.isDoorOpen = !doorController.isDoorOpen;
            }
        }
    }

    //moves switch down
    void MoveSwitchDown()
    {
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchSpeed * Time.deltaTime);
        }
    }

    //moves switch up
    void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchSpeed * Time.deltaTime);
        }
    }

    //trigger to make sure you are on switch
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressingSwitch = !isPressingSwitch;

            if (KeyDoor && HasRequiredItem(requiredItem))
            {
                if (isDoorOpenSwitch && !doorController.isDoorOpen)
                {
                    doorController.isDoorOpen = !doorController.isDoorOpen;
                }
                else if (isDoorCloseSwitch && doorController.isDoorOpen)
                {
                    doorController.isDoorOpen = !doorController.isDoorOpen;
                }
            }

        }


        if (collision.CompareTag("Lassoable"))
        {
            isPressingSwitch = !isPressingSwitch;

            if (PressurePlateDoor)
            {
                if (isDoorOpenSwitch && !doorController.isDoorOpen)
                {
                    doorController.isDoorOpen = !doorController.isDoorOpen;
                }
                else if (isDoorCloseSwitch && doorController.isDoorOpen)
                {
                    doorController.isDoorOpen = !doorController.isDoorOpen;
                }
            }
        }
    }

    //trigger to make sure you are off switch
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
    {
        StartCoroutine(SwitchUpDelay(switchDelay));

        // Check if it's a pressure plate door and the close switch is activated
        if (PressurePlateDoor && isDoorCloseSwitch && doorController.isDoorOpen)
        {
            doorController.isDoorOpen = !doorController.isDoorOpen;
        }
    }
    }

    //adds delay to switch moving up and down
    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isPressingSwitch = false;
    }

    //check to make sure you have required item
    public bool HasRequiredItem(InventoryManager.AllItems itemRequired)
    {
        if (InventoryManager.Instance.inventoryItems.Contains(itemRequired))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
