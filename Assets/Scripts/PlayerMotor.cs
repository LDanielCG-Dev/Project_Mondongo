using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
	#region Properties
	private CharacterController controller;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 0.5f;

	private bool isCrouching = false;

	private bool isSprinting = false;
	private bool isSprintingPressed = false;
	public float sprintSpeed = 6f;
	public float walkSpeed = 4f;

	const float DEFAULT_WALK_SPEED = 5f;
	const float DEFAULT_CROUCH_SPEED = 2f;
	public float DEFAULT_CROUCH_HEIGHT = 0.5f;
	public float DEFAULT_ONFOOT_HEIGHT = 2.0f;

	#endregion

	// Start is called before the first frame update
	void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = controller.isGrounded;
	}

	#region ProcessMove
	// Recieve the inputs from our InputManager.cs and apply them to our character controller.
	public void ProcessMove(Vector2 input, float sprintValue)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

		// Set the speed based on whether the player is sprinting, crouching or walking
		float currentSpeed = isSprinting && !isCrouching && sprintValue > 0.5f ? sprintSpeed : walkSpeed;

		controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
		//controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);


		playerVelocity.y += gravity * Time.deltaTime;
        if (IsGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }
	#endregion

	#region ProcessCrouch
	public void ProcessCrouch(Vector2 input, float crouchValue)
	{
		if (crouchValue > DEFAULT_CROUCH_HEIGHT && !isCrouching)
		{
			// Crouch
			isCrouching = true;
			controller.height = DEFAULT_CROUCH_HEIGHT;
			if (isSprintingPressed)
			{
				walkSpeed = DEFAULT_WALK_SPEED;
			}
			else 
			{
				walkSpeed = DEFAULT_CROUCH_SPEED;
			}
		}
		else if (crouchValue <= DEFAULT_CROUCH_HEIGHT && isCrouching)
		{
			// On foot
			isCrouching = false;
			controller.height = DEFAULT_ONFOOT_HEIGHT;
			walkSpeed = DEFAULT_WALK_SPEED;
		}
	}
	#endregion

	public void Jump()
    {
        if (IsGrounded) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -0.5f * gravity);
        }
    }

	public void StartSprinting()
	{
		if (!isSprintingPressed) // Only sprint if the key is pressed
		{
			isSprintingPressed = true;
			isSprinting = true;
		}
	}

	public void StopSprinting()
	{
		isSprintingPressed = false;
		isSprinting = false;
	}

}

