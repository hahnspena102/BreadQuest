using UnityEngine;

public class Bread : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private int jumpCount;
    private float horizontalInput;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpCount = 0;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount <= 1) {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 8);
            jumpCount += 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (jumpCount == 0) {
            body.linearVelocity = new Vector2(horizontalInput * 15, body.linearVelocity.y);
        } else {
            body.linearVelocity = new Vector2(horizontalInput * 10, body.linearVelocity.y);
        }
        spriteRenderer.flipX = body.linearVelocity.x < 0f;        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            jumpCount = 0;
        }
    }
}
