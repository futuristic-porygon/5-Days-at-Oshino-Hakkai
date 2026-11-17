using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement: MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PauseManager.isGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        rb.linearVelocity = moveInput * playerSpeed;
        animator.SetBool("isWalking", rb.linearVelocity.magnitude > 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastMoveInputX", moveInput.x);
            animator.SetFloat("LastMoveInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX",moveInput.x);
        animator.SetFloat("InputY",moveInput.y);
    }

}
