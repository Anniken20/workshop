using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Buggy atm, but it shows the concept?
public class MaterialSwitch : MonoBehaviour
{
    public Material material1;
    public Material material2;
    public float switchInterval = 2f; // Time interval for material switching in seconds
    public KeyCode switchKey = KeyCode.T; // Key to initiate material switching

    private Renderer rend;
    private bool isSwitching = false;
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
            ToggleSwitching(); // Toggle material switching on button press
        }
    }

    public void ToggleSwitching()
    {
        if (canSwitch)
        {
            isSwitching = !isSwitching;

            if (isSwitching)
            {
                StartCoroutine(SwitchMaterials());
            }
        }
    }

    private IEnumerator SwitchMaterials()
    {
        canSwitch = false;

        while (isSwitching)
        {
            rend.material = material2; // Change to Material2
            yield return new WaitForSeconds(switchInterval);
            rend.material = material1; // Change back to Material1
            yield return new WaitForSeconds(switchInterval);
        }

        canSwitch = true;
    }
}

