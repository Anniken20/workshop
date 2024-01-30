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
    //public Material material1;
    //public Material ;
    private Shader shaderOG;
    private Shader shaderGhost;
    public float switchInterval = 5f; // Time interval for material switching in seconds
    //public KeyCode switchKey = KeyCode.T; // Key to initiate material switching
    public float switchDelay = 5f; // Delay in seconds before switching is allowed again

    private Renderer rend;
    private bool canSwitch = true;

    private bool baseShaderActive = true;
    
    //please be pushed im begging

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
       // rend.material = material1; // Initialize with Material1
        shaderGhost = Shader.Find("Shader Graphs/Ghost Shader");
        shaderOG = Shader.Find("Shader Graphs/LIT TOON");

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
    

   /* public void ToggleShader()
    {
        if (baseShaderActive)
        {
           // baseMatActive = false;
            rend.material = material2;
        } 
        else
        {
            // baseMatActive = true;
            rend.material = material1;
        }
    }*/

    public void SwitchMaterial()
    {
       Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();
       
       foreach (Renderer mat in allMats)
       {
        if (mat.material.shader != null)
        {
            mat.material.shader = shaderGhost; // change to ghostShader
        }
       }
    }

    private IEnumerator SwitchCooldown()
    {
        Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);
        
        foreach (Renderer mat in allMats)
       {
            mat.material.shader = shaderOG; // change back
       }
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