using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    
    public float jumpForce;
    public float originalJumpForce;
    public float moveSpeed;
    public float gravityScale;
    private Vector3 moveDirection;
    
    private Coroutine jumpBoostCoroutine;
    public Transform respawnPoint;
    private PowerUpManager powerUpManager;

    public Animator animator;

    public float knockBackForce;
    public float knockBackDuration;
    private bool isKnockedBack;
    private float knockBackTimer;
    private Vector3 knockBackDirection;

    // Start is called before the first frame update
    void Start()
    {
        powerUpManager = FindObjectOfType<PowerUpManager>();
        characterController = GetComponent<CharacterController>();
        jumpForce = 20f;
        moveSpeed = 6f;
        gravityScale = 5f;
        originalJumpForce = jumpForce;
        knockBackForce = 15f;
        knockBackDuration = 0.3f;
        isKnockedBack = false;
        knockBackTimer = 0f;

    }

    // Update is called once per frame
    void Update()
    {

        if (!isKnockedBack)
        {
            float yTemp = moveDirection.y; // We want to keep the up and down movement of the player as it is
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yTemp;

            if (characterController.isGrounded)
            {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                }
            }

            // Apply gravity
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

            // Apply movement 
            characterController.Move(moveDirection * Time.deltaTime);

        }
        else
        {
            knockBackTimer -= Time.deltaTime;
            if (knockBackTimer <= 0)
            {
                isKnockedBack = false;
            }
            else
            {
                characterController.Move(knockBackDirection * knockBackForce * Time.deltaTime);
            }
        }

        animator.SetBool("isGrounded", characterController.isGrounded); // jumping animation
        animator.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        if (transform.position.y < -20f) // Adjust the y-value as needed
        {
            Respawn();
        }

    }

    public void Powerup(float boostAmount, float duration)
    {
        if (jumpBoostCoroutine != null)
        {
            StopCoroutine(jumpBoostCoroutine);
            jumpForce = originalJumpForce; // Reset to original in case of stacking effects
        }
        jumpBoostCoroutine = StartCoroutine(JumpBoost(boostAmount, duration));
    }

    private IEnumerator JumpBoost(float boostAmount, float duration)
    {
        jumpForce += boostAmount;
        yield return new WaitForSeconds(duration);
        jumpForce = originalJumpForce;
        jumpBoostCoroutine = null;
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        moveDirection = Vector3.zero;

        if (powerUpManager != null)
        {
            powerUpManager.RespawnPowerUps();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Trigger collision detected with: " + other.gameObject.name);
            ApplyKnockback(other);
        }
    }

    private void ApplyKnockback(Collider enemy)
    {
        isKnockedBack = true;
        knockBackTimer = knockBackDuration;
        knockBackDirection = (transform.position - enemy.transform.position).normalized;
        knockBackDirection.y = 1.5f; // Keep the knockback horizontal
    }
}
