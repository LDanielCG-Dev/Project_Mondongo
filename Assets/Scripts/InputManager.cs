using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	#region Properties
	private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

	#endregion

	// Start is called before the first frame update
	#region Awake
	void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump(); // Each time a jump is performed, a callback context calls motor.Jump()
		onFoot.Sprint.started += ctx => motor.StartSprinting();
		onFoot.Sprint.canceled += ctx => motor.StopSprinting();
	}
	#endregion

	// Update is called once per frame\
	#region Update
	void Update()
	{
		// Tell the playerMotor to move using the value from our movement action.
		motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>(), onFoot.Sprint.ReadValue<float>());

		// Tell the playerLook to use the value from our Look action
		look.ProcessLook(onFoot.Look.ReadValue<Vector2>());

		motor.ProcessCrouch(onFoot.Movement.ReadValue<Vector2>(), onFoot.Crouch.ReadValue<float>());
	}
	#endregion

	private void OnEnable()
	{
        onFoot.Enable();
	}
	private void OnDisable()
	{
		onFoot.Disable();
	}
}
