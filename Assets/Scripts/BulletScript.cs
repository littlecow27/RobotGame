using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 5f;
    [SerializeField] private float bulletLifetime = 0f;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NetworkManager.Destroy(gameObject, despawnTime);
        rb.linearVelocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        bulletLifetime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer)
        {
            return;
        }
        if (bulletLifetime <= 0.05f)
        {
            return;
        }
        collision.gameObject.GetComponent<RobotScript>()?.TakeDamage();
        Destroy(gameObject);
    }
}
