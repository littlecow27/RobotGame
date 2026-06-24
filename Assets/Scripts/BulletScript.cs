using Mirror;
using UnityEngine;

public class BulletScript : NetworkBehaviour 
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float despawnTime = 5f;
    [SerializeField] private float bulletLifetime = 0f;
    [SyncVar] public int firedByPlayerNumber;


    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NetworkManager.Destroy(gameObject, despawnTime);
        rb.linearVelocity = transform.right * speed;
        ApplyColor();
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
        if (collision.gameObject.GetComponent<RobotScript>() != null && collision.gameObject.GetComponent<RobotScript>().playerNumber.Equals(firedByPlayerNumber))
        {
            return;
        }
        collision.gameObject.GetComponent<RobotScript>()?.TakeDamage();
        Destroy(gameObject);
    }
    void ApplyColor()
    {
        if(firedByPlayerNumber == 2)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
            var main = GetComponent<ParticleSystem>().main;
            main.startColor = Color.red;
        }
        else if(firedByPlayerNumber == 1)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.HSVToRGB(.51f, .69f, .85f);
            var main = GetComponent<ParticleSystem>().main;
            main.startColor = Color.HSVToRGB(.51f, .69f, .85f);
        }
    }
}
