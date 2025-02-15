using UnityEngine;
using System.Collections;

public class Bread : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;
    private int jumpCount;
    private bool isCrouching, isAttacking;
    private float horizontalInput, verticalInput;
    private Animator animator;
    private float movementSpeed;
    [SerializeField]private AudioSource jumpSFX;
    [SerializeField]private AudioSource attackSFX;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        jumpCount = 0;
    }
    void Update() {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1) {
            isCrouching = false;
            animator.SetBool("crouch",false);
            body.linearVelocity = new Vector2(body.linearVelocity.x, 8);
            jumpCount += 1;
            jumpSFX.Play(0);

        }

        // Crouch
        if (verticalInput < -0.1 && body.linearVelocity.y == 0) {
            animator.SetBool("crouch",true);
            isCrouching = true;
        } else {
            animator.SetBool("crouch",false);
            StartCoroutine(Uncrouch());
        } 

        if (isCrouching) {
            collider.size = new Vector2(1.13f,0.8f);
            collider.offset = new Vector2(0.065f,-0.48f);
        } else {
            collider.size = new Vector2(1.13f,1.6f);
            collider.offset = new Vector2(0.065f,-0.02f);
        }
        // Sword
        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking && !isCrouching) {
            StartCoroutine(Attack());
        }
        
        animator.SetFloat("horizontal",Mathf.Abs(body.linearVelocity.x));
        animator.SetFloat("vertical",body.linearVelocity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (verticalInput >= 0) {
            movementSpeed = 8;
        } else {
            movementSpeed = 4;
        }
        body.linearVelocity = new Vector2(horizontalInput * movementSpeed, body.linearVelocity.y);

        // Turn
        if (horizontalInput < 0) {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        } else if (horizontalInput > 0) {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    IEnumerator Uncrouch() {
        animator.SetBool("crouch",false);
        if (verticalInput < 0) {
            yield return new WaitForSeconds(0f);
        }
        yield return new WaitForSeconds(.4f);
        isCrouching = false;
    }

    IEnumerator Attack() {
        isAttacking = true;
        attackSFX.Play(0);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            jumpCount = 0;
        } else if (collision.gameObject.tag == "Lava") {
            body.position = new Vector2(1f,-1f);
        }
    }
}
