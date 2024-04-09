using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkingSpeed = 7.5f;
    [SerializeField] private float runningSpeed = 11.5f;
    [SerializeField] private float jumpHeight = 8.0f;
    [SerializeField] private float gravity = 20.0f;
    public Camera playerCamera;

    [Header("Grounding")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundedCheck;
    [SerializeField] private float groundedDistance = 2;

    [Header("Camera")]
    [SerializeField] private Transform camHolder;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 45.0f;
    float rotationX = 0;
    Vector2 inputDir;



    public CharacterController characterController { get; private set; }
    public bool isGrounded { get; private set; }

    Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        CheckIfGrounded();
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        MovementHandler(isRunning);
    }

    private void MovementHandler(bool isRunning)
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = 0f;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 fixedMoveDir;

        moveDir *= (isRunning ? runningSpeed : walkingSpeed);
        moveDir = transform.TransformDirection(moveDir);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * -gravity);

        velocity.y -= gravity * Time.deltaTime;

        fixedMoveDir = moveDir;
        fixedMoveDir.y = velocity.y;

        characterController.Move(fixedMoveDir * Time.deltaTime);
    }

    public void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(groundedCheck.position, Vector3.down, groundedDistance, groundMask);
    }


    private void LateUpdate()
    {
        if (!isLocalPlayer) return;

        inputDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        rotationX += -inputDir.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, inputDir.x * lookSpeed, 0);
    }

    private void OnDrawGizmos()
    {
        if (groundedCheck)
            Gizmos.DrawRay(groundedCheck.position, Vector3.down * groundedDistance);
    }
}
