using Mirror;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour 
{
    public Transform[] spawnPoints;
    [SyncVar] public bool gameStarted;
    [SyncVar] public int round = 0;
    [SyncVar] public bool gameOver = false;
    public float restartTimer = 3;
    public float startTimer = 1;
    [SyncVar] public bool playersAssigned = false;
    [SyncVar] public int winner = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spawnPoints.Length != 2) {
            Debug.Log("Error! Spawnpoints are not equal to two.");
        }
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer && !playersAssigned && NetworkManager.singleton.numPlayers == 2)
        {
            AssignSpawnPoints();
        }
        if (!gameStarted && NetworkManager.singleton.numPlayers == 2) {
            StartGame();
        }
        if(gameOver && isServer)
        {
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0)
            {
                restartTimer = 3;
                ResetRound();
                gameOver = false;
            }
        }
    }
    [Server]
    void StartGame() {
        Debug.Log("Game Started!");
        if(startTimer <= 0) {
            gameStarted = true;
        } else {
            startTimer -= Time.deltaTime;
        }
    }
    
    [Server]
    public void EndGame(GameObject loser)
    {
        RobotScript winnerPlayer = null;
        foreach (RobotScript p in FindObjectsByType<RobotScript>(FindObjectsSortMode.InstanceID))
        {
            if (p.gameObject != loser)
            {
                p.AddScore();
                winnerPlayer = p;
            }
        }
        if (winnerPlayer.score >= 3)
        {
            winner = winnerPlayer.playerNumber;
        }
        else
        {
            gameOver = true;
        }
    }
    [Server]
    public void ResetRound()
    {
        round++;
        foreach(RobotScript p in FindObjectsByType<RobotScript>(FindObjectsSortMode.InstanceID))
        {
            p.Respawn();
        }
    }
    [Server]
    void AssignSpawnPoints()
    {
        playersAssigned = true;
        RobotScript[] players = FindObjectsByType<RobotScript>(FindObjectsSortMode.InstanceID);
        if(players.Length != 2)
        {
            return;
        }
        if (players[0].netId > players[1].netId)
        {
            RobotScript temp = players[0];
            players[0] = players[1];
            players[1] = temp;
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].spawnPoint = spawnPoints[i].position;
            players[i].playerNumber = i + 1;
        }
        round++;
    }
}
