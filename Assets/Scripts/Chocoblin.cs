using UnityEngine;
using System.Collections;

public class Chocoblin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField] GameObject fireball;
    [SerializeField] Rigidbody2D player;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 5f;
    private SoundHandler soundHandler;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(StartAttack());

        GameObject sirGluten = GameObject.Find("SirGluten");
        player = sirGluten.GetComponent<Rigidbody2D>();

          soundHandler = FindFirstObjectByType<SoundHandler>(); 

    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset + 0.5f);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        while (Vector2.Distance(body.position, player.position) > 30f) {
            yield return null;
        } 

        animator.SetTrigger("attack");
        //animator.SetBool("onGround", false);
        body.AddForce(new Vector2(0, 1.2f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);
        
        

        Vector2 spawnPosition = new Vector2(body.position.x, body.position.y);
        GameObject newFireball = Instantiate(fireball, spawnPosition, Quaternion.identity);
        newFireball.transform.parent = transform;

        Rigidbody2D fireballBody= newFireball.GetComponent<Rigidbody2D>();
        Vector2 directionToPlayer = new Vector2(player.position.x - body.position.x, 0).normalized;
        if (fireballBody != null) {
            fireballBody.linearVelocity = directionToPlayer * 10f;
        }

        soundHandler.PlaySFX(5);

        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("onGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("onGround", false);
        }
    }
}
