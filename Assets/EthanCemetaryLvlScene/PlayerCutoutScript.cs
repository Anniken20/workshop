
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutoutScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask cutoutMask;

    [SerializeField]
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height - 1);

        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, cutoutMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for(int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", 0.1f);
                materials[m].SetFloat("_Falloffsize", 0.05f);
            }
        }
    }
}

