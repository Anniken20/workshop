using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//NO MORE BUGS WOOO
//All numbers can be edited but too different can make it cause issues in ghost controller too
//BE AWARE

public class MaterialSwitch : MonoBehaviour

{
    public CharacterMovement iaControls;
    private InputAction phase;
    public Material material1;
    public Material material2;
    public float switchInterval = 5f; // Time interval for material switching in seconds
    //public KeyCode switchKey = KeyCode.T; // Key to initiate material switching
    public float switchDelay = 5f; // Delay in seconds before switching is allowed again

    private Renderer rend;
    private bool canSwitch = true;

    private bool baseMatActive = true;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = material1; // Initialize with Material1
    }

    /*
    private void Update()
    {
        if (phase.triggered)
        {
            if (canSwitch)
            {
                SwitchMaterial();
                StartCoroutine(SwitchCooldown());
            }
        }
    }
    */

    public void ToggleMaterial()
    {
        if (baseMatActive)
        {
            baseMatActive = false;
            rend.material = material2;
        } else
        {
            baseMatActive = true;
            rend.material = material1;
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

    private void Awake(){
        iaControls = new CharacterMovement();
    }
    private void OnEnable(){
        phase = iaControls.CharacterControls.Phase;

        phase.Enable();
    }
    private void OnDisable(){
        phase.Disable();
    }
}