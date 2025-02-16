using UnityEngine;
using System.Collections;

public class Goblin : MonoBehaviour
{
    [SerializeField]private int health = 3;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [SerializeField] Rigidbody2D player;
    //private ParticleSystem particleSystem;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();      
        //particleSystem = GetComponent<ParticleSystem>(); 
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
            //particleSystem.Play();
            Destroy(gameObject,0.3f);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sword") {
            Debug.Log("hit!");
            StartCoroutine(Hurt());

        }
    }

    IEnumerator Hurt() {
        health--;

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
