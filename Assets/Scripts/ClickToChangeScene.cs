using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClickToChangeScene : MonoBehaviour
{
    public CharacterMovement iaControls;

    private InputAction jump;
    private InputAction shoot;

    public string sceneName;

    private void Update()
    {
        if (jump.triggered || shoot.triggered)
        {
            Invoke(nameof(LoadScene), 2f);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }
    private void OnEnable()
    {
        shoot = iaControls.CharacterControls.Shoot;
        jump = iaControls.CharacterControls.Jump;

        shoot.Enable();
        jump.Enable();
    }
    private void OnDisable()
    {
        shoot.Disable();
        jump.Disable();
    }
}
