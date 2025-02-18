using UnityEngine;
using System.Collections;

public class GummyPlant : MonoBehaviour
{
    [SerializeField]private int health = 1;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [SerializeField] SirGluten player;
    [SerializeField] ParticleHandler particleHandler;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();      
    }

    void Update(){
        if (health == 0) {
            particleHandler.PlayParticle(body.position.x, body.position.y-0.5f);
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sword") {
            StartCoroutine(Burst()); 
        }
    }

    IEnumerator Burst() {
        StartCoroutine(player.Heal());
        health--;
        yield return null;
    }
}
