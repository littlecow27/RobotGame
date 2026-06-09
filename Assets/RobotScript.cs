using Mirror;
using TMPro;
using UnityEngine;

public class RobotScript : NetworkBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    [SerializeField] private float speedH;
    [SerializeField] private float speedV;
    [SyncVar] private Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isLocalPlayer) {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer) {
            if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) {
                move = new Vector2(Input.GetAxis("Horizontal"), 1);
            } else {
                
                move = new Vector2(Input.GetAxis("Horizontal"), 0);

            }
            if(Input.GetKey(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }
    private void FixedUpdate() {
        if(isLocalPlayer) {
            rb.linearVelocity = new Vector2(move.x * speedH, rb.linearVelocity.y+(move.y * speedV));
        }
    }
}
