using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NO MORE BUGS WOOO
//All numbers can be edited but too different can make it cause issues in ghost controller too
//BE AWARE

public class MaterialSwitch : MonoBehaviour

{
    public Material material1;
    public Material material2;
    public float switchInterval = 5f; // Time interval for material switching in seconds
    public KeyCode switchKey = KeyCode.T; // Key to initiate material switching
    public float switchDelay = 5f; // Delay in seconds before switching is allowed again

    private Renderer rend;
    private bool canSwitch = true;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = material1; // Initialize with Material1
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            if (canSwitch)
            {
                SwitchMaterial();
                StartCoroutine(SwitchCooldown());
            }
        }
    }

    private void SwitchMaterial()
    {
        rend.material = material2; // Change to Material2
    }

    private IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);
        rend.material = material1; // Change back to Material1
        //yield return new WaitForSeconds(switchDelay);
        canSwitch = true;
    }
}

