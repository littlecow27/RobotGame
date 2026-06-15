using Mirror;
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
                if(Input.GetKey(KeyCode.Escape)) {
                    Application.Quit();
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
