using UnityEngine;
using System.Collections;

public class Grahoblin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField] Rigidbody2D player;
    [SerializeField] GameObject sword;
    private float speed = 3f;
    private float stopDistance = 3.5f;
    [SerializeField] private float cooldown = 2f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject sirGluten = GameObject.Find("SirGluten");
        player = sirGluten.GetComponent<Rigidbody2D>();
        
        StartCoroutine(Attack());      
    }

    void FixedUpdate(){
        if (Mathf.Abs(player.position.x - body.position.x) < 12f) {
            Move();
        }
    }
    private void Move(){
        Vector2 direction = new Vector2(player.position.x - body.position.x, 0).normalized;

        if (Vector2.Distance(body.position, player.position) > stopDistance) {
            body.linearVelocity = new Vector2(direction.x * speed, body.linearVelocity.y);
            animator.SetBool("isWalking", true);
        } else {
            body.linearVelocity = new Vector2(direction.x * 0, body.linearVelocity.y);
            animator.SetBool("isWalking", false);
        }

    }
    

    IEnumerator Attack(){
        while (Vector2.Distance(body.position, player.position) > stopDistance) {
            yield return null;
        } 
        yield return new WaitForSeconds(cooldown/2f);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.4f);
        body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        sword.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        sword.SetActive(false);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;


        yield return new WaitForSeconds(cooldown/2f);
        StartCoroutine(Attack());
        
        
    }
}
