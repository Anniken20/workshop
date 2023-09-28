using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitch : MonoBehaviour

{
    public Material material1; // The first material
    public Material material2;

    private Renderer rend;
    private bool useMaterial1 = true;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = material1; // Start with material1
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Toggle between the two materials
            if (useMaterial1)
            {
                rend.material = material2;

            }
            else
            {
                rend.material = material1;
            }

            useMaterial1 = !useMaterial1;
        }
    }
}
