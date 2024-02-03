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
    private Collider objectCollider;
    //please be pushed im begging

    // Ghosty val script
    private GhostController ghostController;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
       // rend.material = material1; // Initialize with Material1
        shaderGhost = Shader.Find("Shader Graphs/Ghost Shader");
        shaderOG = Shader.Find("Shader Graphs/LIT TOON");
        objectCollider = GetComponent<Collider>();

        // Find the ghosty script to the player
        ghostController = GetComponent<GhostController>();
        if(ghostController == null)
        {
            Debug.LogError("You fucking forgot the script on the player loser");
        }
    }
    private void Update()
    {
        if(ghostController != null)
        {
            if(ghostController.IsAbilityEnabled)
            {
                SwitchMaterial(shaderGhost);
                objectCollider.isTrigger = true;
            }
            else
            {
                SwitchMaterial(shaderOG);
                objectCollider.isTrigger = false;
            }
        }
    }
    private void SwitchMaterial(Shader shader)
    {
       Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();
       
       foreach (Renderer mat in allMats)
       {
        if (mat.material.shader != null)
        {
            mat.material.shader = shader; 
        }
       }
    }
    /*private IEnumerator SwitchCooldown()
    {
        Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);
        
        foreach (Renderer mat in allMats)
       {
            mat.material.shader = shaderOG; // change back
       }
        canSwitch = true;
        objectCollider.isTrigger = false; // Resetting the fucking trigger
    }*/

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }
    private void OnEnable()
    {
        if (ghostController != null)
        {
            ghostController.OnAbilityToggled += HandleAbilityToggled;
        }
    }
    private void OnDisable()
    {
        if (ghostController != null)
        {
            ghostController.OnAbilityToggled -= HandleAbilityToggled;
        }
    }
        private void HandleAbilityToggled(bool isAbilityEnabled)
    {
        if (isAbilityEnabled)
        {
            SwitchMaterial(shaderGhost);
            objectCollider.isTrigger = true;
        }
        else
        {
            SwitchMaterial(shaderOG);
            objectCollider.isTrigger = false;
        }
    }
}