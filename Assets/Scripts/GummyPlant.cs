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

        GameObject sirGluten = GameObject.Find("SirGluten");
        player = sirGluten.GetComponent<SirGluten>();   
        particleHandler = FindFirstObjectByType<ParticleHandler>(); 
    }

    void Update(){
        if (health == 0) {
            particleHandler.PlayParticle(body.position.x, body.position.y-0.5f);
            body.position = new Vector2(-10f, -10f);
            Destroy(gameObject,10f);
            health--;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Sword") {
            StartCoroutine(Burst()); 
        }
    }

    IEnumerator Burst() {
        health--;

        StartCoroutine(player.Heal());
        yield return null;
    }
}
