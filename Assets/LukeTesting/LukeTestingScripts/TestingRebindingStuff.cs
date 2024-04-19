using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingRebindingStuff : MonoBehaviour
{
    public PlayerInput _playerInput;
    private InputAction interact;
    //public bool testingBool;
    private void Awake(){
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable(){
        interact = _playerInput.actions["Interact"];
        interact.Enable();
    }
    private void OnDisable(){
        interact.Disable();
    }
    private void Update(){
        //testingBool = interact.WasPressedThisFrame();
        if(interact.triggered){
            Debug.Log("Interact Pressed");
        }
    }
    
}
