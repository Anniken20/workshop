using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindingManager : MonoBehaviour
{
    // Add the refrences here
    public InputActionReference JumpRef, ShootRef, LassoRef, RedirectRef, PhaseRef, InteractRef, OpenRefs,  AimRef, CancelRef;

    // They need to be disabled on enable during runtime or you get issues
    // I don't wanna fight with Unity or C# I'm not that smart
    // Please don't hurt my family

    // This is so stupid and took me way too long, I hate it.
    private void OnEnable()
    {
        JumpRef.action.Disable();
        ShootRef.action.Disable();
        LassoRef.action.Disable();
        RedirectRef.action.Disable();
        PhaseRef.action.Disable();
        InteractRef.action.Disable();
        //OpenRefs.action.Disable(); 
        AimRef.action.Disable();
        CancelRef.action.Disable();
    }
    // Remember to enable them bitches again or you have more issues
    // This is awfully set up but I am really tired of Input systems..
    // Make sure you click and drag them into the Controls Menu section plz
    private void OnDisable()
    {
        JumpRef.action.Enable();
        ShootRef.action.Enable();
        LassoRef.action.Enable();
        RedirectRef.action.Enable();
        PhaseRef.action.Enable();
        InteractRef.action.Enable();
        //OpenRefs.action.Enable();
        AimRef.action.Enable();
        CancelRef.action.Enable();
    }
}