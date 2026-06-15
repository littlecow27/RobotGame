using Mirror;
using Mirror.BouncyCastle.Pqc.Crypto.Falcon;
using TMPro;
using UnityEngine;

public class RobotScript : NetworkBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    [SerializeField] private float speedH;
    [SerializeField] private float maxSpeedV;
    [SerializeField] private float accelerationV;
    [SyncVar] private Color color;
    private GameObject gameManager;
    public GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if(isLocalPlayer) {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager == null) {
            Debug.Log("GameManager not found!");
        }
        if(isLocalPlayer) {
            if(gameManager.GetComponent<GameManager>().gameStarted) {
                rb.gravityScale = 2.0f;
                
                if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) {
                    move = new Vector2(Input.GetAxis("Horizontal"), 1);
                } else {
                    move = new Vector2(Input.GetAxis("Horizontal"), 0);
                }
                if(move.x > 0.1f) {
                    transform.localScale = new Vector3(1, 0);
                } else if(move.x < 0.1f) {
                    transform.localScale = new Vector3(-1, 0);
                }
                if(Input.GetKey(KeyCode.Escape)) {
                    Application.Quit();
                }
                if(Input.GetKey(KeyCode.Z)) {
                    float tempYRot;
                    if(transform.localScale.x > 0) {
                        tempYRot = 0;
                    } else {
                        tempYRot = -180;
                    }
                    Instantiate(bullet, transform.position + new Vector3(1.26f, .09f), new Quaternion(0, 0, 0, ));
                }
            } else {
                rb.gravityScale = 0.0f;
            }
        }
    }
    private void FixedUpdate() {
        if(gameManager.GetComponent<GameManager>().gameStarted) {
            if(isLocalPlayer) {
                float newYVelocity = rb.linearVelocity.y;
                if(move.y > 0.1) {
                    newYVelocity += accelerationV;
                }
                newYVelocity = Mathf.Clamp(newYVelocity, -maxSpeedV, maxSpeedV);
                rb.linearVelocity = new Vector2(move.x * speedH, newYVelocity);
            }
        }
    }
}
