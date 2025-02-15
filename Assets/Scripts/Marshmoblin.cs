using UnityEngine;
using System.Collections;

public class Marshmoblin : MonoBehaviour
{
    private int health;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [SerializeField] GameObject spear;
    [SerializeField] Rigidbody2D player;
    
    void Start()
    {
        health = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        StartCoroutine(Attack());
        
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
            Destroy(gameObject);
        }
    }

    IEnumerator Attack(){
        float upwardOffset = 0.2f;
        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y + 1f);
        Vector2 directionToPlayer = (new Vector2(player.position.x, player.position.y) - (Vector2)transform.position).normalized;
        Vector2 adjustedDirection = new Vector2(directionToPlayer.x, directionToPlayer.y + upwardOffset).normalized;

        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, adjustedDirection);
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = transform;
        Rigidbody2D spearBody= newSpear.GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = newSpear.transform.up * 14f; 
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(Attack());
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
