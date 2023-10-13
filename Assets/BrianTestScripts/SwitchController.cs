using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
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



    void Awake ()
    {
        switchSizeY = transform.localScale.y / 2;

        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressingSwitch)
        { 
            MoveSwitchDown();
        }
        else if (!isPressingSwitch)
        {
            MoveSwitchUp();
        }
    }

    void MoveSwitchDown()
    {
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchSpeed * Time.deltaTime);
        }
    }

     void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressingSwitch = !isPressingSwitch;

            if(HasRequiredItem(requiredItem))
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

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }

    IEnumerator SwitchUpDelay (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isPressingSwitch = false;
    }

    public bool HasRequiredItem(InventoryManager.AllItems itemRequired)
    {
        if(InventoryManager.Instance.inventoryItems.Contains(itemRequired))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
