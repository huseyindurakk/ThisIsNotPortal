using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public bool Forward { get; set; }
    public bool Left { get; set; }
    public bool Backward { get; set; }
    public bool Right { get; set; }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        RegisterInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void RegisterInputActions()
    {
        playerInputActions.Player.Forward.performed += context => Forward = true;
        playerInputActions.Player.Forward.canceled += context => Forward = false;

        playerInputActions.Player.Left.performed += context => Left = true;
        playerInputActions.Player.Left.canceled += context => Left = false;

        playerInputActions.Player.Backward.performed += context => Backward = true;
        playerInputActions.Player.Backward.canceled += context => Backward = false;

        playerInputActions.Player.Right.performed += context => Right = true;
        playerInputActions.Player.Right.canceled += context => Right = false;
    }
}