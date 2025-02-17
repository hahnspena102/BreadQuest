using UnityEngine;

public class MarshSpear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag != "Goblin") {
            Destroy(gameObject,0.05f);
        }
    }
}
