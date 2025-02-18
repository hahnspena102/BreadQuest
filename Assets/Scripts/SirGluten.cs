using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SirGluten : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D sgCollider;
    private int jumpCount;
    private bool isCrouching, isAttacking, isHurting, isSprinting;
    private float horizontalInput, verticalInput;
    private Animator animator;
    private float movementSpeed;
    [SerializeField]private AudioSource jumpSFX;
    [SerializeField]private AudioSource attackSFX;

    // Stats
    [SerializeField]private int maxHealth = 100, health; 
    [SerializeField]private float maxYeast = 5, yeast;

    // Sword Things
    [SerializeField]private GameObject sword;
    // UI
    [SerializeField] private GameObject healthMeter; 
    [SerializeField] private Slider yeastMeter;

    private List<GameObject> hearts = new List<GameObject>();




    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sgCollider = GetComponent<BoxCollider2D>();
        jumpCount = 0;

        yeast = maxYeast;
        health = maxHealth;
        yeastMeter.maxValue = yeast;
        yeastMeter.value = yeast;

        foreach (Transform child in healthMeter.transform)hearts.Add(child.gameObject);
    }

    void Update() {
        // Stats
        foreach (Transform child in healthMeter.transform) child.gameObject.SetActive(false);

        for (int i = 0; i<health;i++) {
            healthMeter.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (jumpCount <= 1) {
            yeast += 20f * Time.deltaTime;
        } else {
            yeast += 5f * Time.deltaTime;
        }
        
        yeast = Mathf.Clamp(yeast, 0, 100);
        yeastMeter.value = yeast;

        if (health == 0) {
            SceneManager.LoadScene(0);
        }

        // Movement
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        

        if (!isHurting) {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space)) {

                if (jumpCount >= 1) {
                    if (yeast <= 40) {
                        return;
                    }
                    yeast -= 40;
                }
                
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
                sgCollider.size = new Vector2(1.13f,0.8f);
                sgCollider.offset = new Vector2(0.065f,-0.48f);
            } else {
                sgCollider.size = new Vector2(1.13f,1.6f);
                sgCollider.offset = new Vector2(0.065f,-0.02f);
            }
            // Sword
            if (Input.GetKeyDown(KeyCode.Return) && !isAttacking && !isCrouching) {
                StartCoroutine(Attack());
            }
        }
        animator.SetFloat("horizontal",Mathf.Abs(body.linearVelocity.x));
        animator.SetFloat("vertical",body.linearVelocity.y);

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        

        // Movement
        if (verticalInput >= 0 && isSprinting) {
            movementSpeed = 12f;
            yeast -= 40f * Time.deltaTime;
        } else if (verticalInput >= 0) {
            movementSpeed = 6f;
        } else {
            movementSpeed = 3f;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        

        if (!isHurting) {
            body.linearVelocity = new Vector2(horizontalInput * movementSpeed, body.linearVelocity.y);

            if (!isAttacking) {
                // Turn
                if (horizontalInput < 0) {
                    Vector2 rotator = new Vector3(transform.rotation.x, 180f);
                    transform.rotation = Quaternion.Euler(rotator);
                } else if (horizontalInput > 0) {
                    Vector2 rotator = new Vector3(transform.rotation.x, 0f);
                    transform.rotation = Quaternion.Euler(rotator);
                }
            }
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
        yield return new WaitForSeconds(0.1f);
        sword.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        sword.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        isAttacking = false;

    }

    IEnumerator Hurt() {
        health--;
        animator.SetTrigger("hurt");
        isHurting = true;

        spriteRenderer.color = Color.red;

        float duration = 0.6f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, elapsedTime / duration);
            yield return null;
        }

        spriteRenderer.color = Color.white;

        isHurting = false;

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            jumpCount = 0;
        } else if (collision.gameObject.tag == "Lava") {
            body.position = new Vector2(1f,-1f);
        } else if (collision.gameObject.tag == "EnemyProj") {
            if (!isHurting) StartCoroutine(Hurt());
        } else if (collision.gameObject.tag == "Goblin") {
            if (!isHurting) {
                Vector2 collisionPoint = collision.transform.position;
                Vector2 direction;

                if (collisionPoint.x > transform.position.x) {
                    direction = new Vector2(-1f, 1f); 
                } else {
                    direction = new Vector2(1f, 1f);
                }

                body.AddForce(direction * 2f, ForceMode2D.Impulse);
                StartCoroutine(Hurt());
            }
            
        }

    }
}
