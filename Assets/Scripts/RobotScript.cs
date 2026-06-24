using Mirror;
using Mirror.BouncyCastle.Pqc.Crypto.Falcon;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RobotScript : NetworkBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    [SerializeField] private float speedH;
    [SerializeField] private float maxSpeedV;
    [SerializeField] private float accelerationV;
    public GameObject bulletSpawnPoint;
    public GameObject[] healthBar;
    public SpriteRenderer[] colorChangers;
    public SpriteRenderer headIndicator;
    [SyncVar] private Color color;
    private GameObject gameManager;
    public GameObject bullet;
    private const float shootCooldown = 1.0f;
    private float shootTimer = 0.0f;
    private const int maxHealth = 3;
    [SyncVar] private bool isDead = false;
    [SyncVar] private int health = maxHealth;
    private int popForce = 10;
    public bool deathHandled = false;
    [SyncVar] public int score;
    [SyncVar] public Vector3 spawnPoint;
    private int lastRoundPlayed;
    [SyncVar] public int playerNumber;
    public TMP_Text playerNumberText;
    public bool facingSet = false;

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
        UpdateHealthBar();
        if (gameManager == null) {
            Debug.Log("GameManager not found!");
        }
        if (!facingSet) {
            facingSet = true;
            if (playerNumber == 2)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                playerNumberText.transform.localScale = new Vector3(-1, 1, 1);
            }
            if(playerNumber == 2)
            {
                foreach(SpriteRenderer sr in colorChangers)
                {
                    sr.color = Color.red;
                }
            }
        }
        //death handling
        if (isDead)
        {
            if(!deathHandled)
            {
                deathHandled = true;
                GetComponent<BoxCollider2D>().enabled = false;
                if (isLocalPlayer)
                {
                    rb.AddForce(new Vector2(0, popForce), ForceMode2D.Impulse);
                }
            }
            return;
        }
        int globalRound = gameManager.GetComponent<GameManager>().round;
        //this respawns the robot when round changes.
        if(globalRound != lastRoundPlayed)
        {
            lastRoundPlayed = globalRound;
            deathHandled = false;
            facingSet = false;
            GetComponent<BoxCollider2D>().enabled = true;
            if (isLocalPlayer && spawnPoint != new Vector3(0, 0, 0))
            {
                rb.gravityScale = 0.0f;
                rb.linearVelocity = Vector2.zero;
                transform.position = spawnPoint;
            }
        }
        //main input handling and physics stuff here
        if(isLocalPlayer) {
            if(gameManager.GetComponent<GameManager>().gameStarted) {
                rb.gravityScale = 2.0f;
               
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) {
                    move = new Vector2(Input.GetAxis("Horizontal"), 1);
                } else {
                    move = new Vector2(Input.GetAxis("Horizontal"), 0);
                }
                if(move.x > 0.1f) {
                    transform.localScale = new Vector3(1, 1, 1);
                    playerNumberText.transform.localScale = new Vector3(1, 1, 1);
                } else if(move.x < -0.1f) {
                    transform.localScale = new Vector3(-1, 1, 1);
                    playerNumberText.transform.localScale = new Vector3(-1, 1, 1);
                }
                if(Input.GetKey(KeyCode.Escape)) {
                    Application.Quit();
                }
                if(shootTimer > 0)
                {
                    shootTimer -= Time.deltaTime;
                    headIndicator.color = Color.black;
                }
                else if(Input.GetKeyDown(KeyCode.Z)) {
                    float direction = transform.localScale.x;
                    Vector3 location = bulletSpawnPoint.transform.position;
                    SpawnBullet(direction, location);
                    shootTimer = shootCooldown;
                }
                else
                {
                    headIndicator.color = Color.red;
                }
            } else {
                rb.gravityScale = 0.0f;
            }
        }
    }
    private void FixedUpdate() {
        if (isDead) return;
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
    [Command]
    void SpawnBullet(float direction, Vector3 location) {
        float tempYRot;
        if(direction > 0) {
            tempYRot = 0;
        } else {
            tempYRot = -180;
        }
        GameObject b = Instantiate(bullet, location, Quaternion.Euler(0, tempYRot, 0));
        NetworkServer.Spawn(b);
    }
    [Server]
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            isDead = true;
            gameManager.GetComponent<GameManager>().EndGame(gameObject);
        }
    }
    void UpdateHealthBar()
    {
        playerNumberText.text = "Player " + playerNumber;
        for (int i = healthBar.Length - 1; i >= 0; i--)
        {
            if (i < health)
            {
                healthBar[i].SetActive(true);
            }
            else
            {
                healthBar[i].SetActive(false);
            }
        }
    }
    [Server]
    public void AddScore()
    {
        score++;
    }
    [Server]
    public void Respawn()
    {
        health = maxHealth;
        isDead = false;
        
    }
}
