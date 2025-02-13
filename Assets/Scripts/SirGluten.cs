using UnityEngine;
using System.Collections;

public class Bread : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private int jumpCount;
    private bool isCrouching;
    private float horizontalInput;
    private float verticalInput;
    private Animator animator;
    private float movementSpeed;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        jumpCount = 0;
    }
    void Update() {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1) {
            isCrouching = false;
            animator.SetBool("crouch",false);
            body.linearVelocity = new Vector2(body.linearVelocity.x, 8);
            jumpCount += 1;

        }

        // Crouch
        if (verticalInput < 0 && !isCrouching) {
            animator.SetBool("crouch",true);
            isCrouching = true;
        } 
        if (verticalInput > 0 && isCrouching) {
            StartCoroutine(Uncrouch());
            
        }
        // Sword
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("*sword swing*");
        }
        
        animator.SetFloat("horizontal",Mathf.Abs(body.linearVelocity.x));
        animator.SetFloat("vertical",body.linearVelocity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isCrouching) {
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
        yield return new WaitForSeconds(.4f);
        isCrouching = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            jumpCount = 0;
        }
    }
}
