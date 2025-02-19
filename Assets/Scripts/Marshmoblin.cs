using UnityEngine;
using System.Collections;

public class Marshmoblin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private Animator animator;
    [SerializeField] GameObject spear;
    [SerializeField] Rigidbody2D player;
    [SerializeField] private float attackOffset = 0f;
    [SerializeField] private float cooldown = 2f;
    private SoundHandler soundHandler;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject sirGluten = GameObject.Find("SirGluten");
        player = sirGluten.GetComponent<Rigidbody2D>();

        soundHandler = FindFirstObjectByType<SoundHandler>(); 

        StartCoroutine(StartAttack());

    }

    IEnumerator StartAttack(){
        yield return new WaitForSeconds(attackOffset + 0.5f);
        StartCoroutine(Attack());
    }

    IEnumerator Attack(){
        while (Vector2.Distance(body.position, player.position) > 16f) {
            yield return null;
        } 
        
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);
        float upwardOffset = 0.2f;
        Vector2 spawnPosition = new Vector2(body.position.x - 0.58f, body.position.y - 0.15f);
        Vector2 directionToPlayer = (new Vector2(player.position.x, player.position.y) - (Vector2)transform.position).normalized;
        Vector2 adjustedDirection = new Vector2(directionToPlayer.x, directionToPlayer.y + upwardOffset).normalized;

        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, adjustedDirection);
        GameObject newSpear = Instantiate(spear, spawnPosition, rotation);
        newSpear.transform.parent = transform;
        Rigidbody2D spearBody= newSpear.GetComponent<Rigidbody2D>();

        soundHandler.PlaySFX(0);
        if (spearBody != null) {
            spearBody.linearVelocity = newSpear.transform.up * 14f; 
        }
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Attack());
    }
}
