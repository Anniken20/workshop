using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public LineRenderer aimLine;

    [Tooltip("1 => 180* of freedom. 0.5 => 90* of freedom.")]
    public float xAngleFreedom;
    [Tooltip("1 => 180* of freedom. 0.5 => 90* of freedom.")]
    public float yAngleFreedom;

    public float sensitivity = 5f;

    [HideInInspector] public bool canAim = true;
    private Vector3 angle;
    private Vector3 modifiedAngle;
    private bool showingAimLine = false;

    private void Awake()
    {
        angle = new Vector3();
        modifiedAngle = new Vector3();
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
             
        float xDelta = Input.GetAxis("Mouse X");
        float yDelta = Input.GetAxis("Mouse Y");

        xDelta *= sensitivity / 50f;
        yDelta *= sensitivity / 50f;

        modifiedAngle.x += xDelta;
        modifiedAngle.y += yDelta;

        modifiedAngle.x = Mathf.Clamp(xDelta, -xAngleFreedom / 2, xAngleFreedom / 2);
        modifiedAngle.y = Mathf.Clamp(yDelta, -yAngleFreedom / 2, yAngleFreedom / 2);

        angle = gameObject.transform.forward;
        angle += modifiedAngle;

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
            gameObject.transform.position.y + 1.5f,
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
