using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaterialSwitch : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction phase;
    private Shader shaderOG;
    private Shader shaderGhost;
    public float switchInterval = 5f;
    private Renderer rend;
    private Collider coll;
    private bool canSwitch = true;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        coll = GetComponent<Collider>(); // Assuming the object has a collider

        shaderGhost = Shader.Find("Shader Graphs/GhostUnlitAttempt");
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

    public void SwitchMaterial()
    {
        Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();

        foreach (Renderer mat in allMats)
        {
            if (mat.material.shader != null)
            {
                mat.material.shader = shaderGhost;
            }
        }

        // Disable collider's interaction with other objects when switching to ghost shader
        if (coll != null)
        {
            coll.isTrigger = true;
        }
    }

    private IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);

        // Enable collider's interaction with other objects when switching back to original shader
        if (coll != null)
        {
            coll.isTrigger = false;
        }

        Renderer[] allMats = rend.GetComponentsInChildren<Renderer>();
        foreach (Renderer mat in allMats)
        {
            mat.material.shader = shaderOG;
        }

        canSwitch = true;
    }

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }

    private void OnEnable()
    {
        phase = iaControls.CharacterControls.Phase;
        phase.Enable();
    }

    private void OnDisable()
    {
        phase.Disable();
    }
}
