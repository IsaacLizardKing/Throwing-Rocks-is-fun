using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform playerHand;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    public float interactionRange = 10f;
    private RockInteract currentRock;
    private bool isHoldingRock = false;
    bool isRunning = false;

    public AudioSource walkSource;
    public AudioSource pickupSource;
    public AudioSource throwSource;

    [Header("Animations")]
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        // {
        //     moveDirection.y = jumpPower;
        // }
        // else
        // {
        // moveDirection.y = movementDirectionY;
        // }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        float forwardValue = new Vector3(curSpeedX, 0, curSpeedY).magnitude;
        _animator.SetFloat("forward", forwardValue);

        if (forwardValue > 0f && !walkSource.isPlaying)
        {
        walkSource.Play();  
        }
    
        else if (forwardValue <= 0f && walkSource.isPlaying)
        {
        walkSource.Stop();  
        }

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetKeyDown(KeyCode.E)) // Pick up rock (if not holding one)
    {
        if (!isHoldingRock) // Only allow picking up if not holding a rock
        {
            Debug.Log("E was pressed for picking up!");
            interact();
        }
    }

    if (Input.GetKeyDown(KeyCode.R)) // Throw rock (if holding one)
    {
        if (isHoldingRock) // Only allow throwing if holding a rock
        {
            Debug.Log("R was pressed for throwing!");
            ThrowRock();
        }
    }
}

void interact()
{
    RaycastHit hit;

    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
    {
        Debug.Log("raycast: " + hit.collider.gameObject.name);
        RockInteract rock = hit.collider.GetComponent<RockInteract>();

        if (rock != null)
        {
            currentRock = rock;
            Debug.Log("Rock in range!");

            // If we're not holding a rock, pick it up
            if (!isHoldingRock)
            {
                currentRock.PickUp(playerCamera.transform);
                isHoldingRock = true;
                pickupSource.Play();
            }
        }
    }
    else
    {
        currentRock = null;
    }

    // If holding the rock, update its position relative to the player
    if (isHoldingRock && currentRock != null)
    {
        currentRock.UpdateHeldPosition(playerCamera.transform.position, playerCamera.transform.rotation);
    }
}

void ThrowRock()
{
    if (currentRock != null)
    {
        Vector3 throwDirection = playerCamera.transform.forward;
        currentRock.Throw(throwDirection);
        isHoldingRock = false;
        throwSource.Play();
    }
}
}