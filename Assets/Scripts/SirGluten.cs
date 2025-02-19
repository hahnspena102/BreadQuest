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
    private bool isCrouching, isAttacking, isHurting, isSprinting, isUnder;
    private bool sprintToggled = true, startSeq;
    private float horizontalInput, verticalInput;
    private Animator animator;
    private float movementSpeed;
    [SerializeField] float jumpHeight;

    [SerializeField]private List<AudioSource> soundEffects = new List<AudioSource>();
    //[SerializeField]private List<ParticleSystem> particles = new List<ParticleSystem>();

    // Stats
    [SerializeField]private int maxHealth = 100, health; 
    [SerializeField]private float maxYeast = 5, yeast;

    // Sword Things
    [SerializeField]private GameObject sword;
    // UI
    [SerializeField] private GameObject healthMeter; 
    [SerializeField] private Slider yeastMeter;
    [SerializeField] private Image yeastFill,yeastBG;

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


        
        StartCoroutine(StartSequence());

    }

    IEnumerator StartSequence() {
        startSeq = true;
        body.linearVelocity = new Vector2(5f,0f);
        yield return new WaitForSeconds(2f);
        startSeq = false;
    }

    void Update() {
        // Stats
        foreach (Transform child in healthMeter.transform) child.gameObject.SetActive(false);

        for (int i = 0; i<health;i++) {
            healthMeter.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (jumpCount <= 1 && !Input.GetKey(KeyCode.LeftShift) && sprintToggled) {
            yeast += 20f * Time.deltaTime;
        } else if (!Input.GetKey(KeyCode.LeftShift) || !sprintToggled) {
            yeast += 5f * Time.deltaTime;
        }
        
        yeast = Mathf.Clamp(yeast, -5f, 100);
        yeastMeter.value = yeast;

        if (yeast < 0) {
            sprintToggled = false;
            yeastFill.color = new Color(0.7615039f,0f,1f);
            yeastBG.color = new Color(0.470689f,0.04227482f,0.47f);
            yeast = 0;
        }
        if (yeast > 40) {
            yeastFill.color = Color.white;
            yeastBG.color = new Color(0.2568085f, 0.2576115f,0.2568085f);
            sprintToggled = true;
        }

        if (health == 0) {
            SceneManager.LoadScene(2);
        }

        // Movement
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        

        if (!isHurting) {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space)) {

                if (isUnder) return;
                
                if (jumpCount >= 1) {
                    if (yeast <= 40) {
                        return;
                    }
                    yeast -= 40;
                    yeast = Mathf.Clamp(yeast, 0f, 100);
                }
                
                
                isCrouching = false;
                animator.SetBool("crouch",false);
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpHeight);
                
                
                jumpCount += 1;
                soundEffects[0].Play(0);

            }

            // Crouch
            if (verticalInput < -0.1 && body.linearVelocity.y == 0) {
                animator.SetBool("crouch",true);
                isCrouching = true;
                soundEffects[3].Play(0);
            } else if (!isUnder) {
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

        isSprinting = Input.GetKey(KeyCode.LeftShift) && yeast > 0 && sprintToggled;

        // Movement
        if (!isCrouching && isSprinting) {
            movementSpeed = 12f;
            yeast -= 40f * Time.deltaTime;
        } else if (!isCrouching) {
            movementSpeed = 8f;
        } else {
            movementSpeed = 3f;
        }

        /*
        if (body.linearVelocity.x > 6f && jumpCount == 0) {
            particles[0].Play();
        } else {
            particles[0].Stop();
        }
        */
        Vector2 frontRayOrigin = (Vector2)transform.position + (Vector2.up * 0.01f) + (Vector2.right * transform.localScale.x * 0.5f);
        Vector2 backRayOrigin = (Vector2)transform.position + (Vector2.up * 0.01f) - (Vector2.right * transform.localScale.x * 0.5f);

        RaycastHit2D frontHit = Physics2D.Raycast(frontRayOrigin, Vector2.up, 1f);
        RaycastHit2D backHit = Physics2D.Raycast(backRayOrigin, Vector2.up, 1f);

        isUnder = (frontHit.collider != null && !frontHit.collider.CompareTag("Player")) || (backHit.collider != null && !backHit.collider.CompareTag("Player"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurting && !startSeq) {
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
        isCrouching = false;
        if (verticalInput < 0) {
            yield return new WaitForSeconds(0f);
        }
        yield return new WaitForSeconds(.4f);
        
    }

    IEnumerator Attack() {
        isAttacking = true;
        soundEffects[1].Play(0);
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
        StopAllAudio();
        soundEffects[2].Play(0);
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

    public IEnumerator Heal() {
        if (health <= 4) {
            health++;
        } else {
            yield break;
        }

        Image heart = healthMeter.transform.GetChild(health - 1).GetComponent<Image>();

        // Correct color definition (ensuring full alpha)
        Color cyan = new Color(148f / 255f, 243f / 255f, 255f / 255f, 1f);
        Color white = new Color(1f, 1f, 1f, 1f);

        float duration = 0.6f;
        float elapsedTime = 0f;

        // Set color before lerping
        spriteRenderer.color = cyan;
        heart.color = cyan;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Ensure smooth interpolation
            spriteRenderer.color = Color.Lerp(cyan, white, t);
            heart.color = Color.Lerp(cyan, white, t);

            yield return null;
        }

        // Ensure final color is set to white
        spriteRenderer.color = white;
        heart.color = white;
    }

    private void StopAllAudio() {   
        foreach (AudioSource audio in soundEffects) audio.Stop();;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            jumpCount = 0;
        } else if (collision.gameObject.tag == "Void") {
            VoidTile voidTile = collision.gameObject.GetComponent<VoidTile>();
            body.position = voidTile.spawnPoint;
            StartCoroutine(Hurt());
        } else if (collision.gameObject.tag == "EnemyProj") {
            if (!isHurting) StartCoroutine(Hurt());
        } else if (collision.gameObject.tag == "Goblin") {
            if (!isHurting) {
                Vector2 collisionPoint = collision.transform.position;
                Vector2 direction;

                if (collisionPoint.x > transform.position.x) {
                    direction = new Vector2(-1f, 2f); 
                } else {
                    direction = new Vector2(1f, 2f);
                }

                body.AddForce(direction * 4f, ForceMode2D.Impulse);
                StartCoroutine(Hurt());
            }
            
        }

    }
    
}
