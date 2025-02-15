using UnityEngine;
using System.Collections;

public class Marshmoblin : MonoBehaviour
{
    private int health;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [SerializeField] GameObject spear;
    
    void Start()
    {
        health = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        StartCoroutine(Attack());
        
    }

    void Update(){
        if (health == 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator Attack(){
        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y + 1f);
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(65,90));
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = transform;
        Rigidbody2D spearBody= newSpear.GetComponent<Rigidbody2D>();
        if (spearBody != null) {
            spearBody.linearVelocity = newSpear.transform.up * 20f; 
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
