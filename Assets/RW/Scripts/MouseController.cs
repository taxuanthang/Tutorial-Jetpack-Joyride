using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MouseController : MonoBehaviour
{
    [Header("Jetpack Config")]
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    private bool isGrounded;
    private bool isDead = false;
    private uint coins = 0;




    [Header("Refs")]
    public ParticleSystem jetpack;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;
    private Rigidbody2D playerRigidbody;
    public TextMeshProUGUI coinsCollectedLabel;
    public Button restartButton;
    public AudioClip coinCollectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
    public ParallaxScroll parallax;






    public void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        jetpackActive = jetpackActive && !isDead;

        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        if (!isDead)
        {
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = forwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;
        }

        if (isDead && isGrounded)
        {
            restartButton.gameObject.SetActive(true);
        }

        //Move background
        parallax.offset = transform.position.x;

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);

        // play SFX
        AdjustFootstepsAndJetpackSound(jetpackActive);


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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else
        {
            HitByLaser(collider);
        }

    }

    void HitByLaser(Collider2D laserCollider)
    {
        // play SFX
        if (!isDead)
        {
            AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
            laserZap.Play();
        }
        //Logic
        isDead = true;
        mouseAnimator.SetBool("isDead", true);

    }
    void CollectCoin(Collider2D coinCollider)
    {
        // play SFX
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);

        //logic
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketMouse");
    }

    void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        // set footStep and jetpack audio base on correspoding codition
        footstepsAudio.enabled = !isDead && isGrounded;
        jetpackAudio.enabled = !isDead && !isGrounded;
        if (jetpackActive)
        {
            jetpackAudio.volume = 1.0f;
        }
        else
        {
            jetpackAudio.volume = 0.5f;
        }
    }


}
