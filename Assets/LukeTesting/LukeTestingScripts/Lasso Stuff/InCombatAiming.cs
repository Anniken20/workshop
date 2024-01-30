using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class InCombatAiming : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction move;
    private Vector2 moving;
    private LassoController player;
    private float playerX;
    private float playerZ;
    [SerializeField] float maxDist;
    private float storedY;
    private Vector3 startPos;

    void Awake()
    {
        iaControls = new CharacterMovement();
        player = FindObjectOfType<LassoController>();
        storedY = transform.position.y;



    }

    void Update()
    {
        moving = move.ReadValue<Vector2>();
    }

    private void OnEnable(){
        move = iaControls.CharacterControls.Move;
        player.GetComponent<ThirdPersonController>()._canMove = true;

        move.Enable();
        playerX = player.transform.position.x;
        playerZ = player.transform.position.z;
    }
    private void OnDisable(){
        move.Disable();
        player.GetComponent<ThirdPersonController>()._canMove = false;
        playerX = 0f;
        playerZ = 0f;
    }

    private void FixedUpdate(){
        //playerX = player.transform.position.x;
        //playerZ = player.transform.position.z;

        playerX += moving.x * 0.2f;
        playerZ += moving.y * 0.2f;
        
        /*if((transform.position.x - player.transform.position.x) > maxDist){
            playerX -= 1f;
        }
        else if((transform.position.x - player.transform.position.x) < maxDist){
            playerX += 1f;
        }

        if((transform.position.z - player.transform.position.z) > maxDist){
            playerZ -= 1f;
        }
        else if((transform.position.z - player.transform.position.z) < maxDist){
            playerZ += 1f;
        }*/
        //transform.position = new Vector3(playerX, transform.position.y, playerZ);
        //Debug.Log(transform.position.x - player.transform.position.x);
        //Debug.Log(transform.position.z - player.transform.position.z);
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));

        /*if((transform.position.x - player.transform.position.x) < maxDist && (transform.position.z - player.transform.position.z) < maxDist){
            Debug.Log("Within Range");
            transform.position = new Vector3(playerX, transform.position.y, playerZ);
        }
        else{
            Debug.Log("Outside Range");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, storedY, player.transform.position.z), Time.deltaTime);
        }*/
        //var distFromParent = transform.position - player.transform.position;
        playerX = Mathf.Clamp(playerX, player.transform.position.x - maxDist, player.transform.position.x + maxDist);
        playerZ = Mathf.Clamp(playerZ, player.transform.position.z - maxDist, player.transform.position.z + maxDist);

        transform.position = new Vector3(playerX, transform.position.y, playerZ);
    }
}
