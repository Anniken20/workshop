using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public LineRenderer aimLine;

    [HideInInspector] public bool canAim = true;
    private Vector3 angle;
    private bool showingAimLine = false;

    private void Awake()
    {
        canAim = true;
    }
    private void Update()
    {
        UpdateAim();
        DrawLine();
    }
    private void UpdateAim()
    {
        //in case locked during menus or game cutscenes etc.
        if (!canAim) return;

        float yDelta = Input.GetAxis("Mouse Y");
        angle.y += yDelta / 5f;
        angle.y = Mathf.Clamp(angle.y, -0.5f, 0.5f);

        /*float xDelta = Input.GetAxis("Mouse X");
        angle.x += xDelta * gameObject.transform.forward.x / 5f;
        angle.x = Mathf.Clamp(angle.x, -0.5f, 0.5f);

        angle.z += xDelta * gameObject.transform.forward.z / 5f;
        angle.z = Mathf.Clamp(angle.z, -0.5f, 0.5f);*/

        //toggle aim draw
        if (Input.GetKeyDown(KeyCode.E))
        {
            showingAimLine = !showingAimLine;
        }

        //recenter aim
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(RecenterAimRoutine());
        }
    }

    private void DrawLine()
    {
        if (!showingAimLine)
        {
            //Debug.Log("eat penis");
            aimLine.positionCount = 0;
            return;
        }

        aimLine.positionCount = 2;
        Vector3 startPos = new Vector3(gameObject.transform.position.x,
            1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);

        //set position 0 slightly in front of player
        aimLine.SetPosition(0, startPos);

        //set position 1 at impact point if collision
        RaycastHit hitData;
        if (Physics.Raycast(startPos, angle, out hitData, 200f))
        {
            aimLine.SetPosition(1, hitData.point);
        }
        //otherwise set far away in that direction
        else
        {
            aimLine.SetPosition(1, angle * 200f);
        }
    }

    private IEnumerator RecenterAimRoutine()
    {
        canAim = false;
        while (Mathf.Abs(Vector3.Magnitude(angle - gameObject.transform.forward)) > 0.0001f)
        {
            //lerp towards forward direction
            angle = Vector3.Lerp(angle, gameObject.transform.forward, 0.2f);
                
            //wait a frame until next loop iteration
            yield return null;
        }
        canAim = true;
    }

    public Vector3 GetAimAngle()
    {
        return angle;
    }
}
