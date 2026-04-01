using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 playerVeocity;

    private bool isGrounded;

    public float speed = 5f;

    public float gravity = -9.8f;

    public float jumpHeight = 1f;

    private bool sprinting = false;

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

        playerVeocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVeocity.y < 0)
            playerVeocity.y = -2f;

        controller.Move(playerVeocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVeocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
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
}
