using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public LineRenderer aimLine;

    [Tooltip("(Roughly) 2 => 90* of freedom. 0.5 => 30* of freedom.")]
    public float xAngleFreedom;
    [Tooltip("(Roughly) 2 => 90* of freedom. 0.5 => 30* of freedom.")]
    public float yAngleFreedom;

    public GameObject aimGuide;
    public float sensitivity = 5f;
    public bool horizontalAim;

    [HideInInspector] public bool canAim = true;
    private Vector3 angle;
    private Vector3 modifiedAngle;
    private bool showingAimLine = false;
    private float xDelta;
    private float yDelta;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        
        //get input from mouse
        xDelta = Input.GetAxis("Mouse X");
        yDelta = Input.GetAxis("Mouse Y");

        //apply sensitivity
        xDelta *= sensitivity / 100;
        yDelta *= sensitivity / 100;

        modifiedAngle.y += yDelta;

        //clamp and save y-value 
        float ySave = Mathf.Clamp(modifiedAngle.y, -yAngleFreedom / 2, yAngleFreedom / 2);

        aimGuide.transform.Rotate(new Vector3(xDelta * 100, 0f, yDelta * 100));

        //clamp x and z
        /*
        aimGuide.transform.eulerAngles = new Vector3(
            Mathf.Clamp(aimGuide.transform.eulerAngles.x, -20, 20f),
            0f,
            Mathf.Clamp(aimGuide.transform.eulerAngles.z, -20, 20f)
            );
        */

        modifiedAngle.x = aimGuide.transform.rotation.x;
        modifiedAngle.z = aimGuide.transform.rotation.z;

        //restore y angle
        modifiedAngle.y = ySave;

        //apply modified angle
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
            aimLine.positionCount = 0;
            return;
        }

        //add 2 positions to line renderer so a line is drawn
        aimLine.positionCount = 2;

        //start line near chest level, a little bit in front of player
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
        //otherwise set position 1 far away in that direction
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
        modifiedAngle = new Vector3(0f, 0f, 0f);
        aimGuide.transform.eulerAngles = modifiedAngle;
        canAim = true;
    }

    public Vector3 GetAimAngle()
    {
        return angle;
    }
}
