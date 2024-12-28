using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f; // Adjust this to change movement speed
    public float sprintSpeed = 10f;
    public float rotationSpeed = 720f;
    public float jumpForce = 5f;


    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody is not attached to the player");
        }
        else 
        {
            Debug.Log("RigidBody successfully attached");
        }
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck is not assigned in the Inspector");
        }
        else
        {
            Debug.Log("GroundCheck successfully assigned");
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Current Animator State: " + stateInfo.IsName("Jump"));

        HandleMovementInput();

        HandleJumping();
    }

    private void HandleMovementInput()
    {
        // Get input from the horizontal and vertical axes (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Create a direction vector
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        if (direction.magnitude >= 0.1f)
        {
            
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            animator.SetFloat("Speed", direction.magnitude * (currentSpeed / sprintSpeed));    
            
        }
        else 
        {
            animator.SetFloat("Speed", 0);
        }

    }

    private void HandleJumping()
    {
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
            }

            

            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                animator.SetBool("IsFalling", true);
            }

            animator.SetBool("IsJumping", true);
        }
    }

}

