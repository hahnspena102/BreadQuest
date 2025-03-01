using UnityEngine;
using System.Collections;

public class Goblin : MonoBehaviour
{
    [SerializeField]private int health = 3;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [SerializeField] Rigidbody2D player;
    [SerializeField] ParticleHandler particleHandler;
    private SoundHandler soundHandler;
    [SerializeField]private int type = 0;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();      

        GameObject sirGluten = GameObject.Find("SirGluten");
        player = sirGluten.GetComponent<Rigidbody2D>();

        particleHandler = FindFirstObjectByType<ParticleHandler>(); 
        soundHandler = FindFirstObjectByType<SoundHandler>(); 
        
    }

    void Update(){
        if (player.position.x - body.position.x > 0) {
            Vector3 rotator = new Vector2(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
        } else {
            Vector2 rotator = new Vector2(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
        }
        if (health == 0) {
            if (type == 1) {
                soundHandler.PlaySFX(1);
            } else if (type == 2) {
                soundHandler.PlaySFX(4);
            } else if (type == 3) {
                soundHandler.PlaySFX(7);
            }
            particleHandler.PlayParticle(body.position.x, body.position.y-0.5f);
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sword") {
            StartCoroutine(Hurt()); 
        }
    }

    IEnumerator Hurt() {
        health--;

        if (type == 2) {
            soundHandler.PlaySFX(3);
        } else if (type == 3) {
            soundHandler.PlaySFX(6);
        }

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
    }
}
