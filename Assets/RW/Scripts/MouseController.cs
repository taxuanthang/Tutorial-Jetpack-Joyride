using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [Header("Jetpack Config")]
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    private bool isGrounded;

    [Header("Refs")]
    public ParticleSystem jetpack;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;
    private Rigidbody2D playerRigidbody;


    public void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        Vector2 newVelocity = playerRigidbody.velocity;
        newVelocity.x = forwardMovementSpeed;
        playerRigidbody.velocity = newVelocity;

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);

    }

    void UpdateGroundedStatus()
    {
        // vẽ ra circle ở một vị trí groundCheck với radius và check trong Layer
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 75.0f;
        }
    }



}
