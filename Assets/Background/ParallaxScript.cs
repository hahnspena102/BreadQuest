using UnityEngine;

public class parallax : MonoBehaviour

{
    [SerializeField] private Rigidbody2D rb;

    Material mat;
    float distance;

    [Range(0f, 0.5f)]
    public float speed= 0f; 
    void Start()
        
    {
        
        mat= GetComponent<Renderer>().material;
        
    }

    void Update()
    {
        speed = rb.linearVelocity.x * 0.0005f;
        distance +=Time.deltaTime* speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);

    } 
}





