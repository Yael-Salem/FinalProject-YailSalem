using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 playerVelocity;

    private bool isGrounded;

    public float speed = 5f;

    public float gravity = -9.8f;

    public float jumpHeight = 1f;

    [Header("Sprint")]
    private bool sprinting = false;

    [Header("Crouching")]
    private bool crouching = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 1f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;

            float p = crouchTimer / 1;

            p *= p;

            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);

            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;

                crouchTimer = 0f;
            }
        }
    }

    // Receives input from InputManager.cs and applies them to player character
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        moveDirection.x = input.x;
        moveDirection.z = input.y; // Translating vertical movement into forward/backward movement

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0f;

        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;

        speed = sprinting ? 8f : 5f;
    }

    public void SprintCancel()
    {
        sprinting = false;

        speed = 5f;
    }
    
    // Function takes a target for the player to move towards in cutscenes and moves the player to it
    public void MoveTowardsTarget(Transform target)
    {
        if (target == null)
            return;

        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = targetPos - transform.position;
        
        // Moving only if the player hasn't arrived at the target yet
        if (direction.sqrMagnitude > 0.01f)
        {
            Vector3 moveVelocity = direction.normalized * speed * Time.deltaTime;
            controller.Move(moveVelocity);
        }

        // Applying gravity to player's movement
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

}
