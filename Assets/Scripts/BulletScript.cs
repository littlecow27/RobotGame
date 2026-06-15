using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : NetworkBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 5f;

    private Rigidbody2D rb;
    private Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, despawnTime);
        if(transform.rotation.y < -1) {
            rb.linearVelocity = new Vector2(-1f * speed, 0);
        } else {
            rb.linearVelocity = new Vector2(1f * speed, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
