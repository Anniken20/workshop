using System.Collections;
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
        coll = GetComponent<Collider>();

        shaderGhost = Shader.Find("Shader Graphs/GhostUnlitAttempt");
        shaderOG = Shader.Find("Shader Graphs/LIT TOON");

        Debug.Log("shaderGhost is " + shaderGhost);
        Debug.Log("shaderOG is " + shaderOG);
    }

    private void Update()
    {
        if (phase.triggered)
        {
            if (canSwitch && IsObjectVisible())
            {
                SwitchMaterial();
                StartCoroutine(SwitchCooldown());
            }
        }
    }

    private bool IsObjectVisible()
    {
        if (rend == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (!GeometryUtility.TestPlanesAABB(planes, rend.bounds))
        {
            // Get in loser we are going phasing
            return false;
        }
        return true;
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

        if (coll != null)
        {
            coll.isTrigger = true;
            // Enable the BulletPassThrough layer during the phase
            gameObject.layer = LayerMask.NameToLayer("BulletPassThrough");
        }
    }

    private IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchInterval);

        if (coll != null)
        {
            coll.isTrigger = false;
            // Disable the BulletPassThrough layer after the cooldown
            gameObject.layer = LayerMask.NameToLayer("Default");
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