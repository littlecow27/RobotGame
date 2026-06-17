using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 5f;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, despawnTime);
        rb.linearVelocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
