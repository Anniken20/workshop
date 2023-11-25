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
    public GameObject parentMat;
  //  public Material material2;
    private Shader originalShader;
    private Shader replacedShader; 
    public float switchInterval = 5f; // Time interval for material switching in seconds
    //public KeyCode switchKey = KeyCode.T; // Key to initiate material switching
    public float switchDelay = 5f; // Delay in seconds before switching is allowed again

    private Renderer rend;
    private bool canSwitch = true;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = material1; // Initialize with Material1
        originalShader = Shader.Find("Shader Graphs/ToonShader");
        replacedShader = Shader.Find("Shader Graphs/Ghost Shader");
        

    }

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

    private void SwitchMaterial()
    {
        Renderer[] allMats = parentMat.GetComponentsInChildren<Renderer>();

        foreach (Renderer mat in allMats){
        if (mat.material.shader != null)
        {
            mat.material.shader = replacedShader;
         // Change to ghotshader
        }
        }
    }

    private IEnumerator SwitchCooldown()
    {   
        Renderer[] allMats = parentMat.GetComponentsInChildren<Renderer>();
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);
        foreach (Renderer mat in allMats)
        {
            mat.material.shader = originalShader;
        }
        //rend.material = material1; // Change back to Material1
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

